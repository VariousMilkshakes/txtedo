# TxtedO

Extendable command bar. Run commands without leaving the keyboard.

## Python API

### Setting up a python module

```
//Import initial ironpython modules
import sys

//If you wish to has access to the
//txtedo python api declare an empty variable "PyAPI"
PyAPI = None

//Define function to run when module is imported by txtedo
//Start has 1 argument to define module settings
def Start(settings):
	settings.command = "Modules Name"
	settings.desc = "Description of module"
	//Return settings back to txtedo
	return settings

//Define function to run when the module is choosen by the user
//Run has 1 argument, unless "settings.hasInitialQuery = false"
def Run(query):
	//Your python script
```python
