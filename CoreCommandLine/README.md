# Consoles

The big thing that I want to understand is how can I do:

- SubCommands
- Typed Parameters like FileInfo


## Test Case

| Library         | Type Conversion | IoC | Sub Apps | Typed Params | 
|-----------------|-----------------|-----|----------|--------------|
| System.Console  | Y               | .   | Y        | Y            |
| Spectre.Console | Y               | Y   | Y        | Y            | 


### Sub Apps
appname orders create bob

### Typed Params (FileInfo)
appname orders import ./file.txt


## Type Conversions
- file info: yes!
