# Welcome

This project hosts the C# implementation of [GEDCOM X](http://www.gedcomx.org) and related extensions.
The modules of this project each address specific aspects of the [GEDCOM X Specification Set](http://www.gedcomx.org/Specifications.html),
including:

* Readers and writers for the [GEDCOM X XML Serialization Format](https://github.com/FamilySearch/gedcomx/blob/master/specifications/xml-format-specification.md).
* Readers and writers for the [GEDCOM X JSON Serialization Format](https://github.com/FamilySearch/gedcomx/blob/master/specifications/json-format-specification.md).
* Readers and writers for the [GEDCOM X File Format](https://github.com/FamilySearch/gedcomx/blob/master/specifications/file-format-specification.md).
* Client-side libraries for reading and writing a GEDCOM X Web service API that conforms to the [GEDCOM X RS Specification](https://github.com/FamilySearch/gedcomx-rs).

## Data Models

The [`Gedcomx.Model`](./Gedcomx.Model/README.md) subproject provides C# classes that correspond to the data types defined by
the [GEDCOM X Conceptual Model](https://github.com/FamilySearch/gedcomx/blob/master/specifications/conceptual-model-specification.md).
These classes are instrumented such that they can be used to read and write both
[XML](https://github.com/FamilySearch/gedcomx/blob/master/specifications/xml-format-specification.md) and
[JSON](https://github.com/FamilySearch/gedcomx/blob/master/specifications/json-format-specification.md).

For more information about GEDCOM X data models, see the [`Gedcomx.Model`](./Gedcomx.Model/README.md) module.

## GEDCOM X Web Services

The [`Gedcomx.Rs.Api`](./Gedcomx.Rs.Api/README.md) module provides support for reading from and writing to a GEDCOM X 
Web service API that conforms to the [GEDCOM X RS Specification](https://github.com/FamilySearch/gedcomx-rs).

## Reading and Writing XML and JSON

The [`Gedcomx.File`](./GEDCOM X File/README.md) subproject provides support for reading and writing the
[GEDCOM X File Format](https://github.com/FamilySearch/gedcomx/blob/master/specifications/file-format-specification.md).

For more information about reading and writing GEDCOM X files,
see [`GEDCOM X File`](./GEDCOM X File/README.md).

Note: The GEDCOM X File project is responsible for managing all reading and writing to and from XML or JSON, and is the module
responsible for all API serialization.

## GEDCOM X Extensions

The [`extensions`](./Gedcomx.Model.Fs/README.md) module provides a place for extensions to GEDCOM X. [FamilySearch](https://familysearch.org) has defined
a set of extensions to the GEDCOM X Conceptual Model and to the GEDCOM X RS specification that comprise the definition of 
[the FamilySearch API](https://developer.familysearch.org/).
 
The [FamilySearch API Client](./FamilySearch.Api/README.md) comprises the developer SDK for the FamilySearch API.

<a name="Use"/>

# Use

Here's how you might use this project.

## NuGet

The GEDCOM X C# artifacts are provided via [NuGet](http://www.nuget.org/).

There are a total of 7 packages, which you can use in anyway you need.

| NuGet Package Id | Purpose | Notes |
|------------------|---------|-------|
| **[FamilySearch.API.SDK](http://www.nuget.org/packages/FamilySearch.API.SDK/)** | This is the main SDK library, and encompasses the ability to work with generic GEDCOM X and FamilySearch specific GEDCOM X data. | This is the FamilySearch GEDCOM X SDK. Use this project to perform all  GEDCOM X operations supported by FamilySearch, or generic GEDCOM X operations. **If you're unsure which library to use, this is the one to use.** |
| [Gedcomx.Model](http://www.nuget.org/packages/Gedcomx.Model/) | Contains the models for GEDCOM X data. | Use this by itself to just work with the GEDCOM X models and data, and not any web services or files. |
| [Gedcomx.Model.Fs](http://www.nuget.org/packages/Gedcomx.Model.Fs/) | Contains FamilySearch specific GEDCOM X model extensions. | Use this by itself to just work with the GEDCOM X FamilySearch extension models and data, and not any web services or files. |
| [Gedcomx.Model.Rs](http://www.nuget.org/packages/Gedcomx.Model.Rs/) | Contains REST specific GEDCOM X model extensions. | Use this by itself to just work with the GEDCOM X REST extension models and data, and not any web services or files. (This project adds atom feed models.) |
| [Gedcomx.API.SDK](http://www.nuget.org/packages/Gedcomx.API.SDK/) | This is the base SDK library, and encompasses the ability to work with GEDCOM X data. | This is the core GEDCOM X SDK project. Use this to perform basic GEDCOM X operations. |
| [Gedcomx.File](http://www.nuget.org/packages/Gedcomx.File/) | Contains libraries to read and write GEDCOM X files and provides core serialization functionality for all web API communications. | Use this by itself to support reading and writing GEDCOM X files (in XML format) or reading and writing GEDCOM X data in serialized format (JSON OR XML), such as in Web API methods. |
| [Gedcomx.Date](http://www.nuget.org/packages/Gedcomx.Date/) | Contains libraries to work with a variety of GEDCOM X dates and formats. | Use this by itself to support GEDCOM X date processing or manipulation. |

##### Finding the Latest Version

To use the latest version of the libraries, visit the NuGet links above.

# Build

Prerequisites:
* Microsoft .NET Framework 4.5.1. The web installer can be downloaded here: <http://www.microsoft.com/en-us/download/details.aspx?id=40773>
* NuGet 2.8. Instructions for installing can be found here: <http://docs.nuget.org/docs/start-here/installing-nuget>

Here's how you could build this project from source:

```
git clone https://github.com/FamilySearch/gedcomx-csharp.git
cd gedcomx-csharp
nuget restore
msbuild
```

