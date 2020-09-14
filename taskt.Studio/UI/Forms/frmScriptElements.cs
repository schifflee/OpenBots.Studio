using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using taskt.Core.Script;
using taskt.Core.UI.Forms;
using taskt.Core.Utilities.FormsUtilities;
using taskt.UI.Forms.Supplement_Forms;
using System.Linq;

namespace taskt.UI.Forms
{
    public partial class frmScriptElements : ThemedForm
    {
        public List<ScriptElement> ScriptElements { get; set; }
        public string ScriptName { get; set; }
        private TreeNode _userElementParentNode;
        private string _emptyValue = "(no default value)";

        #region Initialization and Form Load
        public frmScriptElements()
        {
            InitializeComponent();
        }

        private void frmScriptElements_Load(object sender, EventArgs e)
        {
            //initialize
            ScriptElements = ScriptElements.OrderBy(x => x.ElementName).ToList();
            _userElementParentNode = InitializeNodes("My Task Elements", ScriptElements);
            ExpandUserElementNode();
            lblMainLogo.Text = ScriptName + " elements";
        }

        private TreeNode InitializeNodes(string parentName, List<ScriptElement> elements)
        {
            //create a root node (parent)
            TreeNode parentNode = new TreeNode(parentName);

            //add each item to parent
            foreach (var item in elements)
                AddUserElementNode(parentNode, "<" + item.ElementName + ">", item.ElementValue);

            //add parent to treeview
            tvScriptElements.Nodes.Add(parentNode);

            //return parent and utilize if needed
            return parentNode;
        }

        #endregion

        #region Add/Cancel Buttons
        private void uiBtnOK_Click(object sender, EventArgs e)
        {          
            //return success result
            DialogResult = DialogResult.OK;
        }

        private void uiBtnCancel_Click(object sender, EventArgs e)
        {
            //cancel and close
            DialogResult = DialogResult.Cancel;
        }
        #endregion

        #region Add/Edit Elements
        private void uiBtnNew_Click(object sender, EventArgs e)
        {
            //create element editing form
            frmAddElement addElementForm = new frmAddElement();
            addElementForm.ScriptElements = ScriptElements;

            ExpandUserElementNode();

            //validate if user added element
            if (addElementForm.ShowDialog() == DialogResult.OK)
            {
                //add newly edited node
                AddUserElementNode(_userElementParentNode, addElementForm.txtElementName.Text, addElementForm.ElementValueDT);

                ScriptElements.Add(new ScriptElement
                {
                    ElementName = addElementForm.txtElementName.Text.Replace("<", "").Replace(">", ""),
                    ElementValue = addElementForm.ElementValueDT
                });
            }
        }

        private void tvScriptElements_DoubleClick(object sender, EventArgs e)
        {
            //handle double clicks outside
            if (tvScriptElements.SelectedNode == null)
                return;

            //if parent was selected return
            if (tvScriptElements.SelectedNode.Parent == null)
                return;

            //top node check
            var topNode = GetSelectedTopNode();

            if (topNode.Text != "My Task Elements")
                return;

            ScriptElement element;
            string elementName;
            DataTable elementValue;
            TreeNode parentNode;

            if(tvScriptElements.SelectedNode.Nodes.Count == 0)
            {
                parentNode = tvScriptElements.SelectedNode.Parent;
                elementName = tvScriptElements.SelectedNode.Parent.Text;               
            }
            else
            {
                parentNode = tvScriptElements.SelectedNode;
                elementName = tvScriptElements.SelectedNode.Text;
            }

            element = ScriptElements.Where(x => x.ElementName == elementName.Replace("<", "").Replace(">", "")).FirstOrDefault();
            elementValue = element.ElementValue;

            //create element editing form
            frmAddElement addElementForm = new frmAddElement(elementName, elementValue);
            addElementForm.ScriptElements = ScriptElements;

            ExpandUserElementNode();

            //validate if user added element
            if (addElementForm.ShowDialog() == DialogResult.OK)
            {
                //remove parent
                parentNode.Remove();
                AddUserElementNode(_userElementParentNode, addElementForm.txtElementName.Text, addElementForm.ElementValueDT);
            }
        }

        private void AddUserElementNode(TreeNode parentNode, string elementName, DataTable elementValue)
        {
            //add new node
            var childNode = new TreeNode(elementName);

            for (int i = 0; i < elementValue.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(elementValue.Rows[i][2].ToString()))
                {
                    TreeNode elementValueNode = new TreeNode($"ValueNode{i}");
                    string enabled = elementValue.Rows[i][0].ToString();
                    enabled = enabled == "True" ? "Enabled" : "Disabled";
                    elementValueNode.Text = $"{enabled} - {elementValue.Rows[i][1]} - {elementValue.Rows[i][2]}";
                    childNode.Nodes.Add(elementValueNode);
                }               
            }           

            if (childNode.Nodes.Count == 0)
            {
                TreeNode elementValueNode = new TreeNode($"ValueNodeEmpty");
                elementValueNode.Text = _emptyValue;
                childNode.Nodes.Add(elementValueNode);
            }

            parentNode.Nodes.Add(childNode);
            ExpandUserElementNode();
        }

        private void ExpandUserElementNode()
        {
            if (_userElementParentNode != null)
                _userElementParentNode.Expand();
        }

        private void tvScriptElements_KeyDown(object sender, KeyEventArgs e)
        {
            //handling outside
            if (tvScriptElements.SelectedNode == null)
                return;

            //if parent was selected return
            if (tvScriptElements.SelectedNode.Parent == null)
            {
                //user selected top parent
                return;
            }

            //top node check
            var topNode = GetSelectedTopNode();

            if (topNode.Text != "My Task Elements")
                return;

            //if user selected delete
            if (e.KeyCode == Keys.Delete)
            {
                //determine which node is the parent
                TreeNode parentNode;
                if (tvScriptElements.SelectedNode.Nodes.Count == 0)
                    parentNode = tvScriptElements.SelectedNode.Parent;
                else
                    parentNode = tvScriptElements.SelectedNode;

                //remove parent node
                string elementName = parentNode.Text.Replace("<", "").Replace(">", "");
                ScriptElement element = ScriptElements.Where(x => x.ElementName == elementName).FirstOrDefault();
                ScriptElements.Remove(element);
                parentNode.Remove();
            }
        }

        private TreeNode GetSelectedTopNode()
        {
            TreeNode node = tvScriptElements.SelectedNode;

            while (node.Parent != null)
                node = node.Parent;

            return node;
        }
        #endregion

        private void pnlBottom_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Theme.CreateGradient(pnlBottom.ClientRectangle), pnlBottom.ClientRectangle);
        }
    }
}