using System.Reflection.Metadata;
using System.Xml;
using Microsoft.VisualBasic;

namespace Emulator;

public class Chip8{
    Memory emuMem;

    private bool debugFlag;
    private bool exitFlag;
    public bool DebugFlag{ get{return debugFlag;} set{debugFlag = value;} }
    public bool ExitFlag{ get{return exitFlag;} private set{exitFlag=value;} }
    
    public Chip8(){
        emuMem = new Memory(16, 16, 4096);
        emuMem.SetProgramCounter(0x200);
        ExitFlag = false;
    }

    public void LoadRom(string romPath){
        byte[] fileBytes = File.ReadAllBytes(romPath);

        for(int i = 0; i < fileBytes.Length; ++i){
            emuMem.SetMemValAt((ushort)(i+512), fileBytes[i]); 
        }
    }

    public void ExecuteCycle(){
        ushort pc = emuMem.GetProgramCounter();
        byte mSigByte = emuMem.GetMemValAt(pc);
        byte lSigByte = emuMem.GetMemValAt(pc+1);

        ushort opcode = (ushort)((mSigByte << 8) | lSigByte);
        emuMem.IncrementProgramCounter();
        HandleOpCodes(opcode, pc);
    }

    private void HandleOpCodes(ushort opcode, ushort pc){
        ushort opID = emuMem.GetOpCodeIdentifier(opcode);
        switch(opID){
            case (ushort)OpCodes.x1NNN:{
                ushort jmpVal = emuMem.GetNNNValue(opcode);
                emuMem.SetProgramCounter(jmpVal);
                break;
            }
            case (ushort)OpCodes.x2NNN:{
                ushort subrVal = emuMem.GetNNNValue(opcode);
                emuMem.PushToStack();
                emuMem.SetProgramCounter(subrVal);
                break;
            }
            case (ushort)OpCodes.x3XNN:{
                ushort register = emuMem.GetVxValue(opcode);
                ushort value = emuMem.GetNNValue(opcode);

                if(emuMem.GetMemValAt(register) == value){
                    emuMem.IncrementProgramCounter();
                }

                break;
            }
            case (ushort)OpCodes.x4XNN:{
                ushort register = emuMem.GetVxValue(opcode);
                ushort value = emuMem.GetNNValue(opcode);

                if(emuMem.GetMemValAt(register) != value){
                    emuMem.IncrementProgramCounter();
                }
                break;
            }
            case (ushort)OpCodes.x5XY0:{
                ushort registerX = emuMem.GetVxValue(opcode);
                ushort registerY = emuMem.GetVyValue(opcode);

                if(emuMem.GetMemValAt(registerX) == emuMem.GetMemValAt(registerY)){
                    emuMem.IncrementProgramCounter();
                }

                break;
            }
            case (ushort)OpCodes.x6XNN:{
                ushort register = emuMem.GetVxValue(opcode);
                ushort value = emuMem.GetNNValue(opcode);

                emuMem.SetMemValAt(register, value);
                break;
            }
            case (ushort)OpCodes.x7XNN:{
                ushort register = emuMem.GetVxValue(opcode);
                ushort value = emuMem.GetNNValue(opcode);
                
                ushort newVal = (ushort)(emuMem.GetMemValAt(register) + value);
                emuMem.SetMemValAt(register, newVal);
                break;
            }
            case 0x8000:{
                Handle8KCodes(opcode);
                break;
            }
        }
        
        if(debugFlag){
            Console.WriteLine($"Opcode: {opcode}|{opcode:X}; Initial Program Counter: {pc}");
            
            var currPC = emuMem.GetProgramCounter();
            var initialMem = emuMem.GetMemValAt(pc);
            var currMem = emuMem.GetMemValAt(currPC);
            Console.WriteLine($"Current Program Counter: {currPC}; Initial Mem Val: {initialMem}|{initialMem:X}; Current Mem Val: {currMem}|{currMem:X}");
        }
    }

    private void Handle8KCodes(ushort opcode){
        ushort subID = (ushort)(opcode & (0x000F));
        ushort registerX = emuMem.GetVxValue(opcode);
        ushort registerY = emuMem.GetVyValue(opcode);

        switch(subID){
            case (ushort)OpCodes.x8XY0:{
                ushort val = emuMem.GetMemValAt(registerY);
                emuMem.SetMemValAt(registerX, val);
                break;
            }
            case (ushort)OpCodes.x8XY1:{
                ushort value = (ushort)(emuMem.GetMemValAt(registerX) | emuMem.GetMemValAt(registerY));
                emuMem.SetMemValAt(registerX, value);
                break;
            }
            case (ushort)OpCodes.x8XY2:{
                ushort value = (ushort)(emuMem.GetMemValAt(registerX) & emuMem.GetMemValAt(registerY));
                emuMem.SetMemValAt(registerX, value);
                break;
            }
            case (ushort)OpCodes.x8XY3:{
                ushort value = (ushort)(emuMem.GetMemValAt(registerX) ^ emuMem.GetMemValAt(registerY));
                emuMem.SetMemValAt(registerX, value);
                break;
            }
            case (ushort)OpCodes.x8XY4:{
                ushort value = (ushort)(emuMem.GetMemValAt(registerY) + emuMem.GetMemValAt(registerX));
                if(value > 255u){
                    emuMem.SetMemValAt(0xF, 1);
                }else{
                    emuMem.SetMemValAt(0xF, 0);
                }

                value = (ushort)(value & 0xFF);
                emuMem.SetMemValAt(registerX, value);
                break;
            }
            case (ushort)OpCodes.x8XY5:{
                break;
            }
            case (ushort)OpCodes.x8XY6:{
                break;
            }
        }
    }
}