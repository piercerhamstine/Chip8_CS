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

    public void PushToStack(){
        stack[stackPointer] = programCounter;
        stackPointer += 1;
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

    /// <summary>
    /// Increment program counter by 2.
    ///</summary>
    public void IncrementProgramCounter(){
        programCounter += 2;
    }

    public byte GetMemValAt(int idx){
        ushort x = (ushort)idx;
        return ram[x];
    }

    public void SetMemValAt(ushort idx, ushort value){
        ushort x = (ushort)idx;
        ram[x] = (byte)value; 
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