using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SabaqRemote
{
    public interface IBth
    {
        void Start(string name, int sleepTime, bool readAsCharArray);
        void setbrightness(float val);
        void BthSend(string data);
        //   bool BthStatus();
        void BthCancel();
        void Cancel();
        ObservableCollection<string> PairedDevices();

        void LoadSettings();
        String StoragePaths();

    }
}

