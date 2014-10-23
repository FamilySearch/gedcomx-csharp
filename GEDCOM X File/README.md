# GEDCOM X Java File Format

This project provides support for reading and writing the
[GEDCOM X File Format](https://github.com/FamilySearch/gedcomx/blob/master/specifications/file-format-specification.md).

# NuGet Package Information


| Package ID          | Link                |
|---------------------|---------------------|
| Insert package name | insert package link |

See [the section on using these libraries](../README.md#Use).
Insert content when ready.

To determine the latest version, ~ visit the NuGet link listed above ~.

# Use

Here's how you might write out a GEDCOM X file. See the [`gedcomx-model` documentation](../gedcomx-model/README.md)
for more information about how to build out the model.

```csharp
using(MemoryStream ms = new MemoryStream())
{
	GedcomxOutputStream file = new GedcomxOutputStream(ms);
	Gedcomx gx = ...;

	file.AddResource(gx);
	file.Dispose();

	FileStream output = System.IO.File.OpenWrite(Path.GetTempFileName());
	Byte[] bytes = ms.ToArray();
	output.Write(bytes, 0, bytes.Length);
	output.Dispose();
}
```

Here's how you might read a GEDCOM X file:

```csharp
FileInfo fi = new FileInfo("C:\Sample.gedx");
using (GedcomxFile file = new GedcomxFile(fi))
{
	GedcomxFileEntry gedxEntry = file.Entries.Where(x => x.ZipEntry.FullName == "tree.xml").Single();
    GedcomxFileEntry imageEntry = file.Entries.Where(x => x.ZipEntry.FullName == "person.png").Single();
	Gedcomx gedx = file.ReadResource<Gedcomx>(gedxEntry);
	Stream image = test.GetResourceStream(imageEntry);
}
```