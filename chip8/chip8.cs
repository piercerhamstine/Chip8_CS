namespace Emulator;

public class Chip8{
    private static readonly int VIDEO_WIDTH = 64;
    private static readonly int VIDEO_HEIGHT = 32;

    private byte[] registers = new byte[16];
    private byte[] memory = new byte[4096];
    private ushort indexRegister = 0x00;
    private ushort programCounter = 0x00;
    private ushort[] stack = new ushort[16]; 
    private ushort stackPointer = 0x00;
    private ushort opcode;

    private short delayTimer;
    private short soundTimer;

    private char[] screen = new char[64*32];
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

    // TODO: REFACTOR THIS LATER
    public void Cycle(){
        opcode = (ushort)(memory[programCounter] << 8 | memory[programCounter+1]);
        Console.WriteLine($"{(opcode):X}");

        // if((opcode & 0xF000) == 0x8000) 
        //     Console.WriteLine($"{(opcode & 0xF000):X}");
        
        programCounter += 2;
        switch(opcode & 0xF000){
            // Handle Clear screen and return opcodes
            case 0x0000:{
                switch(opcode & 0x00F0){
                    case (ushort)OpCodes.CLS:{
                        Console.WriteLine("Called Clear Screen: NOT Implemented");
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
            case 0x8000:{
                switch(opcode & 0xF00F){
                    case (ushort)OpCodes.SETVXVY:{
                        break;
                    }
                    case (ushort)OpCodes.SETVXVYOR:{
                        break;  
                    }
                }
                break;
            }

            case (ushort)OpCodes.DRAW:{
                byte xPosition = (byte)(registers[opcode & 0x0F00 >> 8]);
                byte yPosition = (byte)(registers[opcode & 0x00F0 >> 4]);
                byte height = (byte)(opcode & 0x000F);
                ushort pixel;

                registers[0xF] = 0;
                for(int row = 0; row < height; row++){
                    pixel = memory[indexRegister + row];
                    for(int col = 0; col < 8; col++){
                        if((pixel & (0x80 >> col)) != 0){
                            if(screen[xPosition+col+((yPosition+row)*64)] == 1){
                                registers[0xF] = 1;
                            }

                            screen[xPosition+col+((yPosition+row)*64)] ^= (char)1;

                        }
                        // Console.Write($"{Convert.ToString(screen[xPosition+col+((yPosition+row)*64)], 2)}");
                    }
                    // Console.WriteLine();
                }
                break;
            }
            case (ushort)OpCodes.SETI:{
                indexRegister = (ushort)(opcode & 0x0FFF);
                break;
            }
            case (ushort)OpCodes.JMP:{
                programCounter = (ushort)(opcode & 0x0FFF);
                Console.WriteLine($"Jumping to: {programCounter:X}");
                break;
            }
            case (ushort)OpCodes.SEVXNN:{
                byte reg = (byte)((opcode & 0x0F00) >> 8);
                byte val = (byte)(opcode & 0x00FF);

                // Console.WriteLine($"Register: {reg:X}");
                // Console.WriteLine($"Load Value: {val:X}");

                if(registers[reg] == val){
                    Console.WriteLine("VX == NN, SKIPPING");
                    programCounter += 2;
                }

                break;
            }
            case (ushort)OpCodes.SNEVXNN:{
                byte reg = (byte)((opcode & 0x0F00) >> 8);
                byte val = (byte)(opcode & 0x00FF);

                // Console.WriteLine($"Register: {reg:X}");
                // Console.WriteLine($"Load Value: {val:X}");

                if(registers[reg] != val){
                    programCounter += 2;
                }

                break;
            }
            case (ushort)OpCodes.SNEVXVY:{
                byte vx = (byte)((opcode & 0x0F00) >> 8);
                byte vy = (byte)((opcode & 0x00F0) >> 4);

                if(registers[vx] != registers[vy]){
                    programCounter += 2;
                }

                break;
            }
            case (ushort)OpCodes.ADDVXNN:{
                byte reg = (byte)((opcode & 0x0F00) >> 8);
                byte val = (byte)(opcode & 0x00FF);

                registers[reg] += val;
                break;
            }
            case(ushort)OpCodes.SEVXVY:{
                byte vx = (byte)((opcode & 0x0F00) >> 8);
                byte vy = (byte)((opcode & 0x00F0) >> 4);
                
                if(registers[vx] == registers[vy])
                    programCounter += 2;
                
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
                // Console.WriteLine($"Register: {reg:X}");
                // Console.WriteLine($"Load Value: {val:X}");

                registers[reg] = (byte)val;
                break;                
            }
            default:{
                Console.WriteLine($"ERROR: Unknown opcode - 0x{opcode:X}");
                break;
            }
        }

        // LogLastCycle();
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

    private void UpdateDisplay(){
        for(int i = 0; i < 32; ++i){
            for(int j = 0; j < 64; ++j){
                var pixel = screen[i*64+j];

                if(pixel != 0){
                    Console.Write("▓");
                }
                else{
                    Console.Write("░");
                }
            }
            Console.WriteLine();
        }
    }

    public void Run(string filePath){
        Initialize();

        LoadProgram(filePath);

        // while(true){
        //     Cycle();
        // }

        // Opcode testing purposes;
        for(int i = 0; i < 27; ++i){
            Cycle();
        }
        // UpdateDisplay();
    }
}