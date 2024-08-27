namespace Emulator;

public class Chip8{
    Memory emuMem;

    public Chip8(){
        emuMem = new Memory(16, 16, 4096);
    }

    private void ExecuteCycle(){
        ushort pc = emuMem.GetProgramCounter();
        byte lh = emuMem.GetMemValAt(pc);
        byte rh = emuMem.GetMemValAt(pc+1);

        ushort opcode = (ushort)((lh << 8) | rh);

        HandleOpCode(emuMem.GetOpCodeIdentifier(opcode));

        emuMem.SetProgramCounter(pc+2);
    }

    private void HandleOpCode(ushort opcode){
        Console.WriteLine("Not IMP");
    }

    public void Run(string romPath){
    }
}