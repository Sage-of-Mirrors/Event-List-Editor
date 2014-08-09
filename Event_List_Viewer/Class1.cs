using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindViewer;
using WindViewer.Editor;

namespace WindowsFormsApplication1
{
    public class headerClass
    {
        //0x40/64 bytes long
        /*0x00*/
        public int eventOffset;
        /*0x04*/
        public int eventCount;
        /*0x08*/
        public int actorOffset;
        /*0x0C*/
        public int actorCount;
        /*0x10*/
        public int actionOffset;
        /*0x14*/
        public int actionCount;
        /*0x18*/
        public int propertyOffset;
        /*0x1C*/
        public int propertyCount;
        /*0x20*/
        public int floatBankOffset;
        /*0x24*/
        public int floatCount;
        /*0x28*/
        public int intBankOffset;
        /*0x2C*/
        public int intCount;
        /*0x30*/
        public int stringBankOffset;
        /*0x34*/
        public int stringCount;
        /*0x38*/
        public int padding1; //Always 0?
        /*0x3C*/
        public int padding2; //Always 0?


        public List<eventClass> eventList;
        public List<actorClass> actorList;
        public List<actionClass> actionList;
        public List<propertyClass> propertyList;
        public List<float> floatList;
        public List<int> intList;
        public List<string> stringList;
    }

    public class eventClass
    {
        public string eventName;
        public int eventIndex;
        public int unknown1; //Always 1?
        public int eventPriority;
        public int[] actorIndexes;
        public int actorCount;
        public int unknown2; //Always 0?
        public int unknown3; //Always 0?
        public int flagValue;
        public int unknown4; //Always 0?
        public int unknown5; //Always 0?
        public byte eventSound;
        public byte unknownByte1; //Always 0?
        public byte unknownByte2; //Always 0?
        public byte unknownByte3; //Always 0?
        public int unknown6; //Always 0?
        public int unknown7; //Always 0?
        public int unknown8; //Always 0?
        public int eventState;
        public int unknown9; //Always 0?
        public int unknown10; //Always 0?
    }
    
    public class actorClass
    {
        //0x50/80 bytes long
        /*0x00*/ public string name; //0x20/32 byte-space
        /*0x20*/ public int staffIdentifer; //Don't know what this is, J.H. wasn't sure either
        /*0x24*/ public int curActorIndex; //Index of this actor
        /*0x28*/ public int unknown1; //Changes by actor, generally increasing as it goes
        /*0x2C*/ public int staffType; //0 is a special case, 02 is another special case...
        /*0x30*/ public int initialActionIndex; //First action the actor performs
        /*0x34*/ public int unknown2; //Always 0?
        /*0x38*/ public int currentActionIndex; //Probably used while the event is running
        /*0x3C*/ public int actIndex; //Apparently stores the string length of the current action?
        /*0x40*/ public int unknown3; //Always 0?
        /*0x44*/ public byte unknownb1; //Always 0?
        /*0x45*/ public byte unknownb2; //Always 0?
        /*0x46*/ public byte isAdvance; //Engine might set this when the actor is moving onto its next action
        /*0x47*/ public byte unknownb3; //Always 0?
        /*0x48*/ public int unknown4; //Always 0?
        /*0x4C*/ public int  unknown5; //Always 0?
    }

    public class actionClass
    {
        //0x50/80 bytes long
        /*0x00*/
        public string name; //0x20/32 byte-space...
        /*0x20*/
        public int unknown1; //Sometimes 0, sometimes 1, sometimes 2
        /*0x24*/
        public int curActionIndex; //Index of this action
        /*0x28*/
        public int unknown2; //Different in each action. Not always set
        /*0x2C*/
        public int unknown3; //Different in each action. Not always set
        /*0x30*/
        public int unknown4; //Different in each action. Not always set
        /*0x34*/
        public int unknown5; //Different in each action. Not always set
        /*0x38*/
        public int propertyIndex; //Index of a property
        /*0x3C*/
        public int nextActionIndex; //Next action to perform. If it's 0xFFFFFFFF/4294967295 dec, then there is no next action
        /*0x40*/
        public int unknown6; //Always 0?
        /*0x44*/
        public int unknown7; //Always 0?
        /*0x48*/
        public int unknown8; //Always 0?
        /*0x4C*/
        public int unknown9; //Always 0?
    }

    public class propertyClass
    {
        //0x40/64 bytes long
        /*0x00*/
        public string name; //0x20/32 byte-space!
        /*0x20*/
        public int curPropertyIndex; //Index of this property
        /*0x24*/
        public int dataType; //01 means single float; 02 means vector3; 03 means int; 04 means string
        /*0x28*/
        public int dataIndex; //Index of data in respective bank
        /*0x2C*/
        public int stringLength; //Length of data if it's a string, dataType 04
        /*0x30*/
        public int nextPropertyIndex; //Next property? If 0xFFFFFFFF/4294967295 dec, then there is no next
        /*0x34*/
        public int unknown1; //Always 0?
        /*0x38*/
        public int unknown2; //Always 0?
        /*0x3C*/
        public int unknown3; //Always 0?
    }

    public class Parse
    {
        int offset = 0;

        public headerClass parse(byte[] data)
        {
            //Load header
            headerClass header = new headerClass();

            header.eventOffset = (int)Helpers.Read32(data, 0);
            header.eventCount = (int)Helpers.Read32(data, 4);

            header.actorOffset = (int)Helpers.Read32(data, 8);
            header.actorCount = (int)Helpers.Read32(data, 12);

            header.actionOffset = (int)Helpers.Read32(data, 16);
            header.actionCount = (int)Helpers.Read32(data, 20);

            header.propertyOffset = (int)Helpers.Read32(data, 24);
            header.propertyCount = (int)Helpers.Read32(data, 28);

            header.floatBankOffset = (int)Helpers.Read32(data, 32);
            header.floatCount = (int)Helpers.Read32(data, 36);

            header.intBankOffset = (int)Helpers.Read32(data, 40);
            header.intCount = (int)Helpers.Read32(data, 44);

            header.stringBankOffset = (int)Helpers.Read32(data, 48);
            header.stringCount = (int)Helpers.Read32(data, 52);

            header.padding1 = (int)Helpers.Read32(data, 56);
            header.padding2 = (int)Helpers.Read32(data, 60);

            if (header.padding1 != 0)
            {
                Console.Write("Padding1 not 0, it was " + header.padding1 + "\n");
            }

            if (header.padding2 != 0)
            {
                Console.Write("Padding2 not 0, it was " + header.padding2 + "\n");
            }

            #region Load Events
            //Load up event data
            header.eventList = new List<eventClass>();

            offset = header.eventOffset;

            for (int ev = 0; ev < header.eventCount; ev++)
            {
                eventClass loadedEvent = new eventClass();

                loadedEvent.eventName = Helpers.ReadString(data, offset);
                loadedEvent.eventIndex = (int)Helpers.Read32(data, offset + 32);
                loadedEvent.unknown1 = (int)Helpers.Read32(data, offset + 36);
                loadedEvent.eventPriority = (int)Helpers.Read32(data, offset + 40);

                loadedEvent.actorCount = (int)Helpers.Read32(data, offset + 124);
                loadedEvent.actorIndexes = new int[20];

                int indexOffset = offset + 44;
                int indexAdvance = 0;

                for (int actInd = 0; actInd < 20; actInd++)
                {
                    int tempIndex = (int)Helpers.Read32(data, indexOffset + indexAdvance);

                    loadedEvent.actorIndexes[actInd] = tempIndex;

                    indexAdvance += 4;
                }

                loadedEvent.unknown2 = (int)Helpers.Read32(data, offset + 128);
                loadedEvent.unknown3 = (int)Helpers.Read32(data, offset + 132);
                loadedEvent.flagValue = (int)Helpers.Read32(data, offset + 136);
                loadedEvent.unknown4 = (int)Helpers.Read32(data, offset + 140);
                loadedEvent.unknown5 = (int)Helpers.Read32(data, offset + 144);
                loadedEvent.eventSound = Helpers.Read8(data, offset + 148);
                loadedEvent.unknownByte1 = Helpers.Read8(data, offset + 149);
                loadedEvent.unknownByte2 = Helpers.Read8(data, offset + 150);
                loadedEvent.unknownByte3 = Helpers.Read8(data, offset + 151);
                loadedEvent.unknown6 = (int)Helpers.Read32(data, offset + 152);
                loadedEvent.unknown7 = (int)Helpers.Read32(data, offset + 156);
                loadedEvent.unknown8 = (int)Helpers.Read32(data, offset + 160);
                loadedEvent.eventState = (int)Helpers.Read32(data, offset + 164);
                loadedEvent.unknown9 = (int)Helpers.Read32(data, offset + 168);
                loadedEvent.unknown10 = (int)Helpers.Read32(data, offset + 172);

                header.eventList.Add(loadedEvent);

                offset += 176;
            }
            #endregion

            #region Load Actors
            //Loads actors
            header.actorList = new List<actorClass>();

            offset = header.actorOffset;

            for (int act = 0; act < header.actorCount; act++)
            {
                actorClass actor = new actorClass();

                actor.name = Helpers.ReadString(data, offset);
                actor.staffIdentifer = (int)Helpers.Read32(data, offset + 32);
                actor.curActorIndex = (int)Helpers.Read32(data, offset + 36);
                actor.unknown1 = (int)Helpers.Read32(data, offset + 40);
                actor.staffType = (int)Helpers.Read32(data, offset + 44);
                actor.initialActionIndex = (int)Helpers.Read32(data, offset + 48);
                actor.unknown2 = (int)Helpers.Read32(data, offset + 52);
                actor.currentActionIndex = (int)Helpers.Read32(data, offset + 56);
                actor.actIndex = (int)Helpers.Read32(data, offset + 60);
                actor.unknown3 = (int)Helpers.Read32(data, offset + 64);
                actor.unknownb1 = Helpers.Read8(data, offset + 68);
                actor.unknownb2 = Helpers.Read8(data, offset + 69);
                actor.isAdvance = Helpers.Read8(data, offset + 70);
                actor.unknownb3 = Helpers.Read8(data, offset + 71);
                actor.unknown4 = (int)Helpers.Read32(data, offset + 72);
                actor.unknown5 = (int)Helpers.Read32(data, offset + 76);

                header.actorList.Add(actor);

                offset += 80;
            }
            #endregion

            #region Load Actions
            //Loads actions
            header.actionList = new List<actionClass>();

            offset = header.actionOffset;

            for (int acn = 0; acn < header.actionCount; acn++)
            {
                actionClass action = new actionClass();

                action.name = Helpers.ReadString(data, offset);
                action.unknown1 = (int)Helpers.Read32(data, offset + 32);
                action.curActionIndex = (int)Helpers.Read32(data, offset + 36);
                action.unknown2 = (int)Helpers.Read32(data, offset + 40);
                action.unknown3 = (int)Helpers.Read32(data, offset + 44);
                action.unknown4 = (int)Helpers.Read32(data, offset + 48);
                action.unknown5 = (int)Helpers.Read32(data, offset + 52);
                action.propertyIndex = (int)Helpers.Read32(data, offset + 56);
                action.nextActionIndex = (int)Helpers.Read32(data, offset + 60);
                action.unknown6 = (int)Helpers.Read32(data, offset + 64);
                action.unknown7 = (int)Helpers.Read32(data, offset + 68);
                action.unknown8 = (int)Helpers.Read32(data, offset + 72);
                action.unknown9 = (int)Helpers.Read32(data, offset + 76);

                header.actionList.Add(action);

                offset += 80;
            }
            #endregion

            #region Load Properties
            header.propertyList = new List<propertyClass>();

            offset = header.propertyOffset;

            for (int prop = 0; prop < header.propertyCount; prop++)
            {
                propertyClass property = new propertyClass();

                property.name = Helpers.ReadString(data, offset);
                property.curPropertyIndex = (int)Helpers.Read32(data, offset + 32);
                property.dataType = (int)Helpers.Read32(data, offset + 36);
                property.dataIndex = (int)Helpers.Read32(data, offset + 40);
                property.stringLength = (int)Helpers.Read32(data, offset + 44);
                property.nextPropertyIndex = (int)Helpers.Read32(data, offset + 48);
                property.unknown1 = (int)Helpers.Read32(data, offset + 52);
                property.unknown2 = (int)Helpers.Read32(data, offset + 56);
                property.unknown3 = (int)Helpers.Read32(data, offset + 60);

                header.propertyList.Add(property);

                offset += 64; 
            }
            #endregion

            #region Load Floats
            //Load float data
            header.floatList = new List<float>();

            offset = header.floatBankOffset;

            for (int f = 0; f < header.floatCount; f++)
            {
                int tempInt = (int)Helpers.Read32(data, offset);

                float tempFloat = Helpers.ConvertIEEE754Float((uint)tempInt);

                header.floatList.Add(tempFloat);

                offset += 4;
            }
            #endregion

            #region Load Ints
            //Load int data
            header.intList = new List<int>();

            offset = header.intBankOffset;

            for (int i = 0; i < header.intCount; i++)
            {
                int tempInt = (int)Helpers.Read32(data, offset);

                header.intList.Add(tempInt);

                offset += 4;
            }
            #endregion

            #region Load Strings
            /*
            //Load string data. Work-in-progress
            stringList = new List<string>();

            offset = header.stringBankOffset;

            for (int s = 0; s < header.stringCount; s++)
            {
                string tempString = Helpers.ReadString(data, offset);

                stringList.Add(tempString);

                offset += tempString.Length;
            }
            */
            #endregion

            return header;

        }

    }
}
