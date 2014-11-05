GEDCOM X C# Model

This is where the C# classes that correspond to the GEDCOM X data types defined by the
[GEDCOM X Conceptual Model](https://github.com/FamilySearch/gedcomx/blob/master/specifications/conceptual-model-specification.md) live.
You can use these classes to read and write both [GEDCOM X XML](https://github.com/FamilySearch/gedcomx/blob/master/specifications/xml-format-specification.md)
and [GEDCOM X JSON](https://github.com/FamilySearch/gedcomx/blob/master/specifications/json-format-specification.md).

# NuGet Package Information


| Package ID | Link |
|------------|------|
| Gedcomx.Model | http://www.nuget.org/packages/Gedcomx.Model/ |

See [the section on using these libraries](../README.md#Use).

To determine the latest version visit the NuGet link listed above.

# Use

## Building the Model

You can use the GEDCOM X model classes to build out a GEDCOM X document.

````csharp
SourceDescription sourceDescription = new SourceDescription() //describe a source
  .SetTitle("Birth Certificate") //with a title
  .SetCitation(new SourceCitation().SetValue("Citation for Birth Certificate")) //and a citation
  .SetResourceType(ResourceType.PhysicalArtifact) //of a physical artifact
  .SetCreated(DateTime.Parse("8/8/1888")); //created on August 8, 1888

Person person = new Person() //create a person
  .SetSource(sourceDescription) //citing the source
  .SetName("Jane Smith") //named Jane Smith
  .SetGender(GenderType.Female) //female
  .SetFact(new Fact(FactType.Birth, "August 8, 1888", "England")); //born 8/8/1888 in England

Person father = new Person() //create a father
  .SetSource(sourceDescription) //citing the source
  .SetName("William Smith") //named William Smith
  .SetFact(new Fact(FactType.Occupation, "Toll Collector")); //toll collector

Person mother = new Person() //create a mother
  .SetSource(sourceDescription) //citing the source
  .SetName("Sarah Smith"); //named Sarah Smith

Relationship fatherRelationship = new Relationship() //create a relationship
  .SetType(RelationshipType.ParentChild) //of type parent-child
  .SetPerson1(father) //between the father
  .SetPerson2(person); //and the person

Relationship motherRelationship = new Relationship() //create a relationship
  .SetType(RelationshipType.ParentChild) //of type parent-child
  .SetPerson1(mother) //between the mother
  .SetPerson2(person); //and the person
  
Gedcomx gx = new Gedcomx() //create a GEDCOM X document
  .SetPerson(person) //with the person
  .SetPerson(father) //and the father
  .SetPerson(mother) //and the mother
  .SetRelationship(fatherRelationship) //and the father relationship
  .SetRelationship(motherRelationship); //and the mother relationship

```

## XML

Here's how you might write out some GEDCOM X XML:

````csharp
Gedcomx gx = ...; //construct the document
Stream stream = ...; //figure out where you want to write the XML
 
DefaultXmlSerialization serializer = ...;
serializer.Serialize(gx, stream);

```

Here's how you might read some GEDCOM X XML:

````csharp
Stream stream = ...; //find the XML
 
DefaultXmlSerialization serializer = ...;
 
//read the document
Gedcomx gx = serializer.Deserialize<Gedcomx>(stream)

```

## JSON

Here's how you might write out some GEDCOM X JSON:

````csharp
Gedcomx gx = ...; //construct the document
Stream stream = ...; //figure out where you want to write the JSON
 
DefaultJsonSerialization serializer = ...;
 
//write the document to the stream:
Serializer.Serialize(gx, stream);
 
```

## JSON

Here's how you might read some GEDCOM X JSON:

````csharp
Stream stream = ...; //find the JSON
 
DefaultJsonSerialization serializer = ...;
 
Gedcomx gx = serializer.Deserialize<Gedcomx>(stream);
```

## The Example Test Suite --- Update Links

For a suite of examples on how to use the model classes, see 
[the `org.gedcomx.examples` test suite](../Gedcomx.Rs.Api.Test/Examples). Many of the tests have
an associated document in [the GEDCOM X recipe book](http://www.gedcomx.org/Recipe-Book.html).`