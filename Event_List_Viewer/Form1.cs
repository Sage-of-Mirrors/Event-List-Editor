using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WindViewer;

namespace WindowsFormsApplication1
{
    public partial class propertyListBox : Form
    {
        byte[] data;

        public List<eventClass> events;

        public List<actorClass> actors;

        public List<actionClass> actions;

        public List<propertyClass> properties;

        public List<float> floatList;

        public List<int> intList;

        public List<string> stringList;

        int selectedEvent = 0;

        int selectedActor = 0;

        int selectedAction = 0;

        int selectedProperty = 0;

        int[] actorIndexes = new int[64]; 

        int[] actionIndexes = new int[64];

        int[] propertyIndexes = new int[64];

        public propertyListBox()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            open();
        }

        private void open()
        {
            DialogResult result = openFileDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                data = Helpers.LoadBinary(openFileDialog1.FileName);

                Parse parser = new Parse();

                var fileHeader = parser.parse(data);

                events = fileHeader.eventList;
                actors = fileHeader.actorList;
                actions = fileHeader.actionList;
                properties = fileHeader.propertyList;
                floatList = fileHeader.floatList;
                intList = fileHeader.intList;

                fillEventListBox();

                eventListBox.SelectedIndex = 0;

                fillEventTextBoxes();

                fillActorListBox();
            }
        }

        private void fillEventListBox()
        {
            foreach (eventClass ev in events)
            {
                eventListBox.Items.Add(ev.eventIndex + ". " + ev.eventName);
            }
        }

        private void fillEventTextBoxes()
        {
            clearEventTextBoxes();

            eventNameBox.Text = events[selectedEvent].eventName;
            eventUnknown1Box.Text = events[selectedEvent].unknown1.ToString();
            eventPriorityBox.Text = events[selectedEvent].eventPriority.ToString();
            eventActorCountBox.Text = events[selectedEvent].actorCount.ToString();
            eventUnknown2Box.Text = events[selectedEvent].unknown2.ToString();
            eventUnknown3Box.Text = events[selectedEvent].unknown3.ToString();
            eventFlagBox.Text = events[selectedEvent].flagValue.ToString();
            eventUnknown4Box.Text = events[selectedEvent].unknown4.ToString();
            eventUnknown5Box.Text = events[selectedEvent].unknown5.ToString();
            eventSoundBox.Text = events[selectedEvent].eventSound.ToString();
        }

        private void clearEventTextBoxes()
        {
            eventNameBox.Clear();
            eventUnknown1Box.Clear();
            eventPriorityBox.Clear();
            eventActorCountBox.Clear();
            eventUnknown2Box.Clear();
            eventUnknown3Box.Clear();
            eventFlagBox.Clear();
            eventUnknown4Box.Clear();
            eventUnknown5Box.Clear();
            eventSoundBox.Clear();
        }

        private void eventListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedEvent = eventListBox.SelectedIndex;

            fillEventTextBoxes();

            fillActorListBox();

            actorListBox.SelectedIndex = 0;
        }

        private void fillActorListBox()
        {
            actorListBox.Items.Clear();

            Array.Clear(actorIndexes, 0, actorIndexes.Length);

            for (int act = 0; act < events[selectedEvent].actorCount; act++)
            {
                actorListBox.Items.Add(actors[events[selectedEvent].actorIndexes[act]].curActorIndex + ". " + actors[events[selectedEvent].actorIndexes[act]].name);

                actorIndexes[act] = actors[events[selectedEvent].actorIndexes[act]].curActorIndex;
            }
        }

        private void fillActorTextBoxes()
        {
            clearActorTextBoxes();

            actorNameBox.Text = actors[actorIndexes[selectedActor]].name;
            actorStaffBox.Text = actors[actorIndexes[selectedActor]].staffIdentifer.ToString();
            actorUnknown1Box.Text = actors[actorIndexes[selectedActor]].unknown1.ToString();
            actorFirstActionBox.Text = actors[actorIndexes[selectedActor]].initialActionIndex.ToString();
        }

        private void clearActorTextBoxes()
        {
            actorNameBox.Clear();
            actorStaffBox.Clear();
            actorUnknown1Box.Clear();
            actorFirstActionBox.Clear();
        }

        private void fillPropertyTextBoxes()
        {
            clearPropertyTextBoxes();

            if (actions[actionIndexes[selectedAction]].propertyIndex != -1)
            {
                propNameBox.Text = properties[propertyIndexes[selectedProperty]].name;
                propDatTypeBox.Text = properties[propertyIndexes[selectedProperty]].dataType.ToString();
                propDatIndexBox.Text = properties[propertyIndexes[selectedProperty]].dataIndex.ToString();
                propDatLengthBox.Text = properties[propertyIndexes[selectedProperty]].stringLength.ToString();
                propNextBox.Text = properties[propertyIndexes[selectedProperty]].nextPropertyIndex.ToString();

                if (properties[propertyIndexes[selectedProperty]].dataType == 3)
                {
                    propDataBox.Text = intList[properties[propertyIndexes[selectedProperty]].dataIndex].ToString();
                }

                if (properties[propertyIndexes[selectedProperty]].dataType == 1)
                {
                    propDataBox.Text = floatList[properties[propertyIndexes[selectedProperty]].dataIndex].ToString();
                }
            }

            else
            {
                propNameBox.Text = "<None>";
            }
        }

        private void clearPropertyTextBoxes()
        {
            propNameBox.Clear();
            propDatTypeBox.Clear();
            propDatIndexBox.Clear();
            propDatLengthBox.Clear();
            propDataBox.Clear();
            propNextBox.Clear();
        }

        private void fillActionListBox()
        {
            int nextActionIndex = actors[actorIndexes[selectedActor]].initialActionIndex;

            actionListBox.Items.Clear();

            Array.Clear(actionIndexes, 0, actionIndexes.Length);

            int i = 0;

            do
            {
                actionListBox.Items.Add(actions[nextActionIndex].curActionIndex + ". " + actions[nextActionIndex].name);

                actionIndexes[i] = actions[nextActionIndex].curActionIndex;

                nextActionIndex = actions[nextActionIndex].nextActionIndex;

                i += 1;
            } while (nextActionIndex != -1);
        }

        private void fillActionTextBoxes()
        {
            clearActionTextBoxes();

            actionNameBox.Text = actions[actionIndexes[selectedAction]].name;
            actionIDBox.Text = actions[actionIndexes[selectedAction]].unknown1.ToString();
            actionUnknown2Box.Text = actions[actionIndexes[selectedAction]].unknown2.ToString();
            actionUnknown3Box.Text = actions[actionIndexes[selectedAction]].unknown3.ToString();
            actionUnknown4Box.Text = actions[actionIndexes[selectedAction]].unknown4.ToString();
            actionUnknown5Box.Text = actions[actionIndexes[selectedAction]].unknown5.ToString();
            actionPropertyBox.Text = actions[actionIndexes[selectedAction]].propertyIndex.ToString();
            actionNextBox.Text = actions[actionIndexes[selectedAction]].nextActionIndex.ToString();
        }

        private void clearActionTextBoxes()
        {
            actionNameBox.Clear();
            actionIDBox.Clear();
            actionUnknown2Box.Clear();
            actionUnknown3Box.Clear();
            actionUnknown4Box.Clear();
            actionUnknown5Box.Clear();
            actionPropertyBox.Clear();
            actionNextBox.Clear();
        }

        private void fillPropertyListBox()
        {
            propListBox.Items.Clear();

            int newPropertyIndex = actions[actionIndexes[selectedAction]].propertyIndex;

            if (newPropertyIndex == -1)
            {
                propListBox.Items.Add("No properties");
            }

            else
            {
                int i = 0;

                do
                {
                    propListBox.Items.Add(properties[newPropertyIndex].curPropertyIndex + ". " + properties[newPropertyIndex].name);

                    propertyIndexes[i] = properties[newPropertyIndex].curPropertyIndex;

                    i += 1;

                    newPropertyIndex = properties[newPropertyIndex].nextPropertyIndex;
                }
                while (newPropertyIndex != -1);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void actorListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedActor = actorListBox.SelectedIndex;

            fillActorTextBoxes();

            fillActionListBox();

            actionListBox.SelectedIndex = 0;
        }

        private void actionListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedAction = actionListBox.SelectedIndex;

            fillActionTextBoxes();

            fillPropertyListBox();

            propListBox.SelectedIndex = 0;
        }

        private void propListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedProperty = propListBox.SelectedIndex;

            fillPropertyTextBoxes();
        }
    }
}
