import sys
sys.path.append("modules/Lib")
from System.Collections.Generic import List
import clr
clr.AddReference('txtedo')
from txtedo.Module.Control import *

PyAPI = None

def Start(messenger):
    messenger.command = "settings"
    messenger.desc = "View and Modify txtedo settings"
    messenger.hasInitialQuery = False;
    return messenger

def Run():
    allSettings = PyAPI.getSettings()
    print allSettings[0].key
    newPreview = List[PreviewItem]()

    i = 0
    while i < allSettings.Count:
        print "loop"
        setting = allSettings[i];
        newPreview.Add(PyAPI.newPreviewItem(setting.key, setting.value));
        i += 1

    print newPreview[0].name
    PyAPI.previewCustomList(newPreview)

    return False