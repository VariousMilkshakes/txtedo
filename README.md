# TxtedO

Smart, slick and black

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

# Operation Hydra

Once c# has started a python module it will try and grab the return value.
If the module is "smooth-flat" it will complete and return a value. No problems will occur.
If the module is "rough-flat" it will not have time to complete the module and txtedo will grab a null value.
If the module is "-spiky" it will try to interact with the c# thread. As the thread has already moved on it will
crash when python trys to modify it. (This problem does not occur after a breakpoint has been triggered; something
to do with the speed of execution while stepping through).
User interaction will not work in our current python module system

## Multithreading

### Stage 1: 2 headed

A thread handler needs to be built. It needs to be able to run and interact with the module.
Before a module is run it needs to be assigned to a thread. The thread will then Run() the
module. This will cause the module to be isolated within the thread (may bring up more issues later).
The txtedo process can then be stopped and wait for the "Active Module" Thread to finish.