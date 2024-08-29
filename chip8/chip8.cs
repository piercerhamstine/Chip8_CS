namespace Emulator;

public class Chip8{
    Memory emuMem;

    bool debugFlag;

    public Chip8(){
        emuMem = new Memory(16, 16, 4096);
    }

    private void ExecuteCycle(){
        ushort pc = emuMem.GetProgramCounter();
        byte mSigByte = emuMem.GetMemValAt(pc);
        byte lSigByte = emuMem.GetMemValAt(pc+1);

        ushort opcode = (ushort)((mSigByte << 8) | lSigByte);

        HandleOpCodes(opcode, pc);

        emuMem.SetProgramCounter(pc+2);
    }

    private void HandleOpCodes(ushort opcode, ushort pc){
        if(debugFlag){
            Console.WriteLine($"Opcode: {opcode}|{opcode:X}; Program Counter: {pc}");
        }

        ushort opID = emuMem.GetOpCodeIdentifier(opcode);
        switch(opID){
            case (ushort)OpCodes.x1NNN:{
                ushort jmpVal = emuMem.GetNNNValue(opcode);
                emuMem.SetProgramCounter(jmpVal);
                break;
            }
            case (ushort)OpCodes.x2NNN:{
                ushort subrVal = emuMem.GetNNNValue(opcode);
                emuMem.PushToStack(subrVal);
                emuMem.SetProgramCounter(subrVal);
                break;
            }
        }
    }

    public void Run(string romPath, bool debugFlag = false){
        this.debugFlag = debugFlag;
    }
}