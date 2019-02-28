using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyOverlay.Controls.FileManagement
{
    public static class FileDialog
    {
        /// <summary>
        /// The filter used for file dialogs.
        /// </summary>
        public static string Filter = "Configuration Files (*.xml)|*.xml|All files (*.*)|*.*";

        /// <summary>
        /// Shows a file dialog to open a file.
        /// </summary>
        /// <param name="path">The path of the file opened.</param>
        /// <returns>Dialog result</returns>
        public static bool Open(out string path)
        {
            var dialog = new OpenFileDialog
            {
                Filter = Filter,
            };
            path = null;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                path = dialog.FileName;
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Shows a file dialog to save a file.
        /// </summary>
        /// <param name="path">The path of the file opened.</param>
        /// <returns>Dialog result</returns>
        public static bool Save(out string path, string defaultName = null)
        {
            var dialog = new SaveFileDialog
            {
                Filter = Filter,
                FileName = defaultName,
            };
            path = null;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                path = dialog.FileName;
                return true;
            }
            else
                return false;
        }
    }
}
