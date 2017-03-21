﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FScruiser.Core.Services;
using System.Windows.Forms;
using FSCruiser.Core;

namespace FSCruiser.WinForms
{
    public class WinFormsDialogService : IDialogService
    {
        #region IDialogService Members

        public bool AskCancel(string message, string caption, bool defaultCancel)
        {
            return MessageBox.Show(message,
                caption,
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.None,
                (defaultCancel) ? MessageBoxDefaultButton.Button2 : MessageBoxDefaultButton.Button1)
                == DialogResult.Cancel;
        }

        FormCruiserSelection _cruiserSelectionView;

        public void AskCruiser(FSCruiser.Core.Models.Tree tree)
        {
            if (ApplicationSettings.Instance.EnableCruiserPopup)
            {
                if (_cruiserSelectionView == null)
                {
                    _cruiserSelectionView = new FormCruiserSelection();
                }
#if NetCF
                _cruiserSelectionView.ShowDialog(tree);
#else
                _cruiserSelectionView.ShowDialog(Form.ActiveForm, tree);
#endif
            }
        }

        public bool AskYesNo(string message, string caption)
        {
            return AskYesNo(message, caption, true);
        }

        public bool AskYesNo(string message, string caption, bool defaultNo)
        {
            return MessageBox.Show(message
                , caption
                , MessageBoxButtons.YesNo
                , MessageBoxIcon.Question
                , (defaultNo) ? MessageBoxDefaultButton.Button2 : MessageBoxDefaultButton.Button1)
                == DialogResult.Yes;
        }

        public void ShowMessage(string message)
        {
            ShowMessage(message, string.Empty);
        }

        public void ShowMessage(string message, string caption)
        {
            MessageBox.Show(message
                , caption
                , MessageBoxButtons.OK
                , MessageBoxIcon.None
                , MessageBoxDefaultButton.Button1);
        }

        #endregion IDialogService Members
    }
}