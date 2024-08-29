namespace Emulator;

public class Memory{
    public ushort registerCount;
    public ushort stackSize;
    public uint memorySize;

    private ushort indexRegister;
    private ushort programCounter;

    private ushort[] registers;
    private byte[] ram;

    private ushort[] stack;
    private byte stackPointer;

    // private byte delayTimer;
    // private byte soundTimer;

    public Memory(ushort regCount, ushort stackSize, uint memSize){
        this.registerCount = regCount;
        this.memorySize = memSize;
        this.stackSize = stackSize;

        ram = new byte[memorySize];
        registers = new ushort[registerCount];
        stack = new ushort[stackSize];
    }

    public void SetStackPointer(int sp){
        stackPointer = (byte)sp;
    }

    public void SetIndexRegister(int i){
        indexRegister = (ushort)i;
    }

    public void SetProgramCounter(int pc){
        programCounter = (ushort)pc;
    }

    public ushort GetProgramCounter(){
        return programCounter;
    }

    public byte GetMemValAt(int idx){
        ushort i = (ushort)idx;
        return ram[i];
    }

    public void PushToStack(ushort val){
        stack[stackPointer] = programCounter;
        stackPointer += 1;
    }

    public ushort GetOpCodeIdentifier(ushort opcode){
        ushort op = (ushort)(opcode & 0xF000 >> 12);
        return op;
    }

    public ushort GetVxValue(ushort opcode){
        ushort vx = (ushort)((opcode & 0x0F00) >> 8); 
        return vx;
    }

    public ushort GetVyValue(ushort opcode){
        ushort vy = (ushort)((opcode & 0x00F0) >> 4);
        return vy;
    }

    public ushort GetNNValue(ushort opcode){
        ushort value = (ushort)(opcode & 0x00FF);
        return value;
    }

    public ushort GetNNNValue(ushort opcode){
        ushort value = (ushort)(opcode & 0x0FFF);
        return value;
    }
}