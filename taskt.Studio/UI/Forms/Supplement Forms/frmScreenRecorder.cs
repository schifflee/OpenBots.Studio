using System;
using System.Collections.Generic;
using System.Drawing;
using taskt.Commands;
using taskt.Core.Command;
using taskt.Core.Settings;
using taskt.Properties;
using taskt.UI.Forms;
using taskt.UI.Forms.ScriptBuilder_Forms;
using taskt.Utilities;

namespace taskt.UI.Supplement_Forms
{
    public partial class frmScreenRecorder : UIForm
    {
        public frmScriptBuilder CallBackForm { get; set; }
        private List<ScriptCommand> _scriptCommandList;
        public bool IsCommandItemSelected { get; set; }
        private ApplicationSettings _appSettings;

        public frmScreenRecorder()
        {
            _appSettings = new ApplicationSettings();
            _appSettings = _appSettings.GetOrCreateApplicationSettings();

            InitializeComponent();
        }

        private void OnHookStopped(object sender, EventArgs e)
        {
            //isRecording = false;
            GlobalHook.HookStopped -= new EventHandler(OnHookStopped);

            //if (!isRecording)
            //{
            //    return;
            //}

            pnlOptions.Show();
            lblRecording.Hide();
            FinalizeRecording();
        }

        private void FinalizeRecording()
        {
            string sequenceComment = $"UI Sequence Recorded {DateTime.Now}";

            _scriptCommandList = GlobalHook.GeneratedCommands;
            var outputList = new List<ScriptCommand>();

            if (chkGroupIntoSequence.Checked)
            {
                var newSequence = new SequenceCommand();

                foreach (ScriptCommand cmd in _scriptCommandList)
                    newSequence.ScriptActions.Add(cmd);

                if (newSequence.ScriptActions.Count > 0)
                    outputList.Add(newSequence);
            }
            else if (chkGroupMovesIntoSequences.Checked)
            {
                var newSequence = new SequenceCommand();
                newSequence.v_Comment = sequenceComment;

                foreach (ScriptCommand cmd in _scriptCommandList)
                {

                    if (cmd is SendMouseMoveCommand)
                    {
                        var sendMouseCmd = (SendMouseMoveCommand)cmd;
                        if (sendMouseCmd.v_MouseClick != "None")
                        {
                            outputList.Add(newSequence);
                            newSequence = new SequenceCommand();
                            newSequence.v_Comment = sequenceComment;
                            outputList.Add(cmd);
                        }
                        else
                            newSequence.ScriptActions.Add(cmd);
                    }
                    else if (cmd is SendKeystrokesCommand)
                    {
                        outputList.Add(newSequence);
                        newSequence = new SequenceCommand();
                        newSequence.v_Comment = sequenceComment;
                        outputList.Add(cmd);
                    }
                    else
                        newSequence.ScriptActions.Add(cmd);
                }

                if (newSequence.ScriptActions.Count > 0)
                    outputList.Add(newSequence);
            }

            else
                outputList = _scriptCommandList;

            var commentCommand = new AddCodeCommentCommand
            {
                v_Comment = sequenceComment
            };
            outputList.Insert(0, commentCommand);

            if (_appSettings.ClientSettings.InsertCommandsInline && IsCommandItemSelected)
                outputList.Reverse();               

            foreach (var cmd in outputList)
                CallBackForm.AddCommandToListView(cmd);

            Close();
        }

        private void btnStartRecording_Click(object sender, EventArgs e)
        {

        }

        private void btnStopRecording_Click(object sender, EventArgs e)
        {
            GlobalHook.StopHook();
            //FinalizeRecording();
        }

        private void frmSequenceRecorder_Load(object sender, EventArgs e)
        {

        }

        private void chkGroupIntoSequences_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chkCaptureMouse_CheckedChanged(object sender, EventArgs e)
        {
            chkGroupMovesIntoSequences.Checked = chkCaptureMouse.Checked;
            chkGroupMovesIntoSequences.Enabled = chkGroupMovesIntoSequences.Checked;
        }

        private void uiBtnRecord_Click(object sender, EventArgs e)
        {
            if (uiBtnRecord.DisplayText == "Start")
            {
                Height = 120;
                Width = 500;
                BringToFront();
                MoveFormToBottomRight(this);
                TopMost = true;
                pnlOptions.Hide();
                lblRecording.Show();
                FormBorderStyle = 0;
                uiBtnRecord.DisplayText = "Stop";
                uiBtnRecord.Image = Resources.various_stop_button;
                uiBtnRecord.Location = new Point(lblRecording.Right + 5, lblRecording.Location.Y);

                int.TryParse(txtHookResolution.Text, out int samplingResolution);

                GlobalHook.HookStopped += new EventHandler(OnHookStopped);
                GlobalHook.StartScreenRecordingHook(chkCaptureClicks.Checked, chkCaptureMouse.Checked,
                    chkGroupMovesIntoSequences.Checked, chkCaptureKeyboard.Checked, chkCaptureWindowEvents.Checked,
                    chkActivateTopLeft.Checked, chkTrackWindowSize.Checked, chkTrackWindowsOpenLocation.Checked,
                    samplingResolution, txtHookStop.Text);
                lblRecording.Text = "Press '" + txtHookStop.Text + "' key to stop recording!";
                // WindowHook.StartHook();

                _scriptCommandList = new List<ScriptCommand>();               
            }
            else
                GlobalHook.StopHook();
        }

        private void chkActivateTopLeft_CheckedChanged(object sender, EventArgs e)
        {
            if (chkActivateTopLeft.Checked)
                chkTrackWindowsOpenLocation.Checked = false;
        }

        private void chkTrackWindowsOpenLocation_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTrackWindowsOpenLocation.Checked)
                chkActivateTopLeft.Checked = false;
        }
    }
}











