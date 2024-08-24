namespace Emulator;

public class Chip8{
    private byte[] registers = new byte[16];
    private byte[] memory = new byte[4096];
    private ushort indexRegister = 0x00;
    private ushort programCounter = 0x00;
    private ushort[] stack = new ushort[16]; 
    private ushort stackPointer = 0x00;
    private ushort opcode;

    private short delayTimer;
    private short soundTimer;

    // private char[] screen = new char[64*32];
    // private short[] keys = new short[16];

    private readonly ushort entryAddress = 0x200;

    public Chip8(){
        Initialize();
    }

    private void Initialize(){
        memory = new byte[4096];
        programCounter = entryAddress;
        indexRegister = 0;
        stackPointer = 0;
        opcode = 0;
    }

    public void Cycle(){
        opcode = (ushort)(memory[programCounter] << 8 | memory[programCounter+1]);
        switch(opcode & 0xF000){
            // Handle Clear screen and return opcodes
            case 0x000:{
                switch(opcode & 0x00F0){
                    case (ushort)OpCodes.CLS:{
                        Console.WriteLine("Called Clear Screen: NOT Implemented");
                        programCounter += 2;
                        break;
                    }
                    case (ushort)OpCodes.RTS:{
                        stackPointer -= 1;
                        programCounter = stack[stackPointer];

                        // Debug
                        Console.WriteLine($"Return Subroutine");
                        break;
                    }
                }
                break;
                
            }

            case (ushort)OpCodes.DRAW:{
                Console.WriteLine("Called Draw: NOT Implemented");
                programCounter += 2;
                break;
            }
            case (ushort)OpCodes.SETI:{
                indexRegister = (ushort)(opcode & 0x0FFF);
                programCounter += 2;
                break;
            }
            case (ushort)OpCodes.RTS:{
                break;
            }
            case (ushort)OpCodes.JMP:{
                programCounter = (ushort)(opcode & 0x0FFF);
                break;
            }
            case (ushort)OpCodes.SEVXNN:{
                byte reg = (byte)((opcode & 0x0F00) >> 8);
                byte val = (byte)(opcode & 0x00FF);

                Console.WriteLine($"Register: {reg:X}");
                Console.WriteLine($"Load Value: {val:X}");

                if(reg == val)
                    programCounter += 4;
                else
                    programCounter += 2;

                break;
            }
            case (ushort)OpCodes.SNEVXNN:{
                byte reg = (byte)((opcode & 0x0F00) >> 8);
                byte val = (byte)(opcode & 0x00FF);

                Console.WriteLine($"Register: {reg:X}");
                Console.WriteLine($"Load Value: {val:X}");

                if(reg != val){
                    programCounter += 4;
                }
                else{
                    programCounter += 2;
                }

                break;
            }
            case (ushort)OpCodes.ADDVXNN:{
                byte reg = (byte)((opcode & 0x0F00) >> 8);
                byte val = (byte)(opcode & 0x00FF);

                registers[reg] += val;
                programCounter+=2;
                break;
            }
            case (ushort)OpCodes.CALLNNN:{
                stack[stackPointer] = (byte)programCounter;
                stackPointer += 1;
                programCounter = (ushort)(opcode & 0x0FFF);

                Console.WriteLine("Call Subroutine");
                break;
            }
            case (ushort)OpCodes.LDVXNN:{
                byte reg = (byte)((opcode & 0x0F00) >> 8);
                byte val = (byte)(opcode & 0x00FF);
                Console.WriteLine($"Register: {reg:X}");
                Console.WriteLine($"Load Value: {val:X}");

                registers[reg] = (byte)val;

                programCounter += 2;
                break;                
            }
            default:{
                Console.WriteLine($"ERROR: Unknown opcode - 0x{opcode:X}");
                programCounter += 2;
                break;
            }
        }

        LogLastCycle();
    }

    private void LogLastCycle(){
        Console.WriteLine($"OPCODE: {opcode:X}, PC: {programCounter:X}, I: {indexRegister:X}, SP: {stackPointer}");
        Console.WriteLine("--------------");
    }

    private void LoadProgram(string filePath){
        byte[] fileBytes = File.ReadAllBytes(filePath);

        for(int i = 0; i < fileBytes.Length; ++i){
            memory[i+512] = fileBytes[i];
        }
    }

    public void Run(string filePath){
        Initialize();

        LoadProgram(filePath);

        // while(true){
        //     Cycle();
        // }

        // Opcode testing purposes;
        for(int i = 0; i < 50; ++i){
            Cycle();
        }
    }
}