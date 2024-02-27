Description:
Create templates and save them in repository, create files 
  and directories from that template


| TagTemplate                                 | Args          | Description                                                          |
| ------------------------------------------- | ------------- | -------------------------------------------------------------------- |
| {{{_SomePlaceholder_}}}                     | Text          | Replacing tag with text                                              |
| \[\[\[_SomeCondition_]]]                    | Boolean       | Including text between tags, if enabled                              |
| !!!_sArray_!!!                              | Number (size) | Repeating text between tags `value(sArray)` times                    |
| !!!_sArray_._xSubArray_!!!                  | Number (size) | Repeating text between tags `value(sArray) * value(xSubArray)` times |
| {{{_sArray_._Placeholder_}}}                | Text          | Replacing tag with text `value(sArray)` times                        |
| \[\[\[_sArray_._Cond_]]]                    | Boolean       | Including text between tags, if enabled `value(sArray)` times        |
| {{{_sArray_._xSubArray_._SubPlaceholder_}}} | Text          | Replacing tag with text `value(sArray) * value(xSubArray)` times     |

| Command              | Description                                      |
| -------------------- | ------------------------------------------------ |
| create `name` `path` | Create formatted files from template recursively |
| init `name` `path`   | Initialize template configuratioin at given path |
| validate `path`      | Validate template at given path                  |
| save `path`          | Save template from given path in repository      |
| remove `name`        | Remove template from repository                  |
| list                 | List all templates stored in repository          |

- [x] Parsing configuration
- [x] Parsing metadata from each file in template
- [x] Parsing args from CLI input and configuration default values
- [x] Formatter: Array tag support
- [x] Formatter: Placeholder tag support
- [x] Formatter: Conditions tag support
- [x] Formatter: Infinite array members support (Array/SubArray/SubSubArray tags)
- [x] Templates repository
- [x] Default values from configuration
- [ ] Prompts for values from configuration
- [ ] Formatting files by their name. Example: !!!Models!!! will be created `value(Models)` times, or {{{}}}
- [ ] Remote template repository's