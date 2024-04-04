namespace TCMS.GUI.Utilities;

public class DialogCloseRequestedEventArgs : EventArgs
{
    public bool? DialogResult { get; }

    public DialogCloseRequestedEventArgs(bool? dialogResult)
    {
        DialogResult = dialogResult;
    }
}
