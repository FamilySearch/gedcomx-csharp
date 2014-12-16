# FamilySearch API SDK

This is a C# library that provides support for consuming the [FamilySearch API](https://developer.familysearch.org/).

The FamilySearch API is an implementation of the [GEDCOM X RS Specification](https://github.com/FamilySearch/gedcomx-rs),
plus some custom FamilySearch extensions. As such, the FamilySearch API SDK extends the
[GEDCOM X RS Client](../Gedcomx.Rs.Api/README.md).


# NuGet Package Information


| Package ID | Link |
|------------|------|
| FamilySearch.API.SDK | http://www.nuget.org/packages/FamilySearch.API.SDK/ |

See [the section on using these libraries](../README.md#Use).

To determine the latest version visit the NuGet link listed above.

# Use

The FamilySearch API SDK extends the [GEDCOM X RS Client](../Gedcomx.Rs.Api/README.md). In addition to the
base functionality of the [GEDCOM X RS Client](../Gedcomx.Rs.Api/README.md), the
FamilySearch API SDK provides additional model classes, convenience classes, and methods to support FamilySearch-specific
functionality.

The FamilySearch API uses [hypermedia as the engine of application state](http://en.wikipedia.org/wiki/HATEOAS)
and so using a client feels like browsing the web. As such, the SDK feels like using a screen scraper. The process can generally
be summarized as follows:

#### Step 1: Read the "Home" Collection
 
Web sites have a "home page". The FamilySearch API has "home collections". Examples of collections include the FamilySearch Family Tree,
FamilySearch Memories, and FamilySearch Records.

#### Step 2: Follow the Right Link

You get stuff done on a web site by following links to where you want to go. Ditto for the FamilySearch API.

#### Step 3: Repeat

Follow more links to get more stuff done.

<a name="exampmles"/>

## Examples

Need something to sink your teeth into?

* [Read the FamilySearch Family Tree (Start Here)](#read-ft)
* [Read a Family Tree Person by Persistent ID](#read-person-by-persistent-id)
* [Read a Family Tree Person by Family Tree ID](#read-person-by-pid)
* [Read a Family Tree Person by Family Tree ID, Including Relationships](#read-pwr-by-pid)
* [Search for Persons or Person Matches in the Family Tree](#search-ft)
* [Create a Person in the Family Tree](#create-ft-person)
* [Create a Couple Relationship in the Family Tree](#create-ft-couple)
* [Create a Child-and-Parents Relationship in the Family Tree](#create-ft-chap)
* [Create a Source](#create-source)
* [Create a Source Reference](#create-source-reference)
* [Read Everything Attached to a Source](#read-references-to-source)
* [Read Person for the Current User](#read-current-user-person)
* [Read Source References](#read-source-references)
* [Read Persona References](#read-evidence-references)
* [Read Discussion References](#read-discussion-references)
* [Read Notes](#read-notes)
* [Read Parents, Children, or Spouses](#read-relatives)
* [Read Ancestry or Descendancy](#read-pedigree)
* [Read Person Matches (i.e., Possible Duplicates)](#read-matches)
* [Declare Not a Match](#declare-not-a-match)
* [Add a Name or Fact](#add-conclusion)
* [Update a Name, Gender, or Fact](#update-conclusion)
* [Create a Discussion](#create-discussion)
* [Attach a Discussion](#create-discussion-reference)
* [Attach a Photo to a Person](#attach-photo)
* [Read FamilySearch Memories](#read-fs-memories)
* [Upload Photo or Story or Document](#add-an-artifact)
* [Create a Memory Persona](#create-memory-persona)
* [Create a Persona Reference](#create-persona-reference)
* [Attach a Photo to Multiple Persons](#multi-person-photo-attach)


<a name="read-ft"/>

### Read the FamilySearch Family Tree (Start Here)

Before you do anything, you need to start by reading the collection that you want
to read or update.

May we suggest you start with the FamilySearch Family Tree?

```csharp
Boolean useSandbox = true; //whether to use the sandbox reference.
String username = "...";
String password = "...";
String developerKey = "...";

//read the Family Tree
FamilySearchFamilyTree ft = new FamilySearchFamilyTree(useSandbox);

//and authenticate.
ft.AuthenticateViaOAuth2Password(username, password, developerKey);
```

<a name="read-person-by-persistent-id"/>

### Read a Family Tree Person by Persistent ID

```csharp
String username = "...";
String password = "...";
String developerKey = "...";

String ark = ...; //e.g. "https://familysearch.org/ark:/61903/4:1:KW8W-RF8"
FamilyTreePersonState person = new FamilyTreePersonState(new Uri(ark))
  .AuthenticateViaOAuth2Password(username, password, developerKey);
```

<a name="read-person-by-pid"/>

### Read a Family Tree Person by Family Tree ID

```csharp
String pid = ...; //e.g. "KW8W-RF8"

FamilySearchFamilyTree ft = ...;
FamilyTreePersonState person = ft.ReadPersonById(pid);
```

<a name="read-pwr-by-pid"/>

### Read a Family Tree Person by Family Tree ID, Including Relationships

```csharp
String pid = ...; //e.g. "KW8W-RF8"

FamilySearchFamilyTree ft = ...;
FamilyTreePersonState person = ft.ReadPersonWithRelationshipsById(pid);
```

<a name="search-ft"/>

### Search for Persons or Person Matches in the Family Tree

```csharp
FamilySearchFamilyTree ft = ...;

//put together a search query
GedcomxPersonSearchQueryBuilder query = new GedcomxPersonSearchQueryBuilder()
  //for a John Smith
  .SetName("John Smith")
  //born 1/1/1900
  .SetBirthDate("1 January 1900")
  //son of Peter.
  .SetFatherName("Peter Smith");

//search the collection
PersonSearchResultsState results = ft.SearchForPersons(query);
//iterate through the results...
List<Entry> entries = results.Results.Entries;
//read the person that was hit
PersonState person = results.ReadPerson(entries[0]);

//search the collection for matches
PersonMatchResultsState matches = ft.SearchForPersonMatches(query);
//iterate through the results...
entries = results.Results.Entries;
//read the person that was matched
person = results.ReadPerson(entries[0]);
```

<a name="create-ft-person"/>

### Create a Person in the Family Tree

```csharp
FamilySearchFamilyTree ft = ...;

//add a person
PersonState person = ft.AddPerson(new Person()
  //named John Smith
  .SetName(new Name("John Smith", new NamePart(NamePartType.Given, "John"), new NamePart(NamePartType.Surname, "Smith")))
  //male
  .SetGender(GenderType.Male)
  //born in chicago in 1920
  .SetFact(new Fact(FactType.Birth, "1 January 1920", "Chicago, Illinois"))
  //died in new york 1980
  .SetFact(new Fact(FactType.Death, "1 January 1980", "New York, New York")),
  //with a change message
  FamilySearchOptions.Reason("Because I said so.")
);
```

<a name="create-ft-couple"/>

### Create a Couple Relationship in the Family Tree

```csharp
FamilySearchFamilyTree ft = ...;

PersonState husband = ...;
PersonState wife = ...;

RelationshipState coupleRelationship = ft.AddSpouseRelationship(husband, wife, FamilySearchOptions.Reason("Because I said so."));
```

<a name="create-ft-chap"/>

### Create a Child-and-Parents Relationship in the Family Tree

```csharp
FamilySearchFamilyTree ft = ...;

PersonState father = ...;
PersonState mother = ...;
PersonState child = ...;

ChildAndParentsRelationshipState chap = ft.AddChildAndParentsRelationship(child, father, mother, FamilySearchOptions.Reason("Because I said so."));
```

<a name="create-source"/>

### Create a Source

```csharp
FamilySearchFamilyTree ft = ...;

//add a source description
SourceDescriptionState source = ft.AddSourceDescription(new SourceDescription()
  //about some resource.
  .SetAbout("http://familysearch.org/ark:/...")
  //with a title.
  .SetTitle("Birth Certificate for John Smith")
  //and a citation
  .SetCitation("Citation for the birth certificate")
  //and a note
  .SetNote(new Note().SetText("Some note for the source.")),
  //with a change message.
  FamilySearchOptions.Reason("Because I said so.")
);
```

<a name="create-source-reference"/>

### Create a Source Reference

```csharp
//the person that will be citing the record, source, or artifact.
PersonState person = ...;

SourceDescriptionState source = ...;

person.AddSourceReference(source, FamilySearchOptions.Reason("Because I said so.")); //cite the source.
```

<a name="read-references-to-source"/>

### Read Everything Attached to a Source

```csharp
//the source.
SourceDescriptionState source = ...;

SourceDescriptionState attachedReferences = ((FamilySearchSourceDescriptionState)source).QueryAttachedReferences();

//iterate through the persons attached to the source
List<Person> persons = attachedReferences.Entity.Persons;
```

<a name="read-current-user-person"/>

### Read Person for the Current User

```csharp
FamilySearchFamilyTree ft = ...;

PersonState person = ft.ReadPersonForCurrentUser();
```

<a name="read-source-references"/>

### Read Source References

```csharp
//the person on which to read the source references.
PersonState person = ...;

//load the source references for the person.
person.LoadSourceReferences();

//read the source references.
List<SourceReference> sourceRefs = person.Person.Sources;
```

<a name="read-evidence-references"/>

### Read Persona References

```csharp
//the person on which to read the persona references.
PersonState person = ...;

//load the persona references for the person.
person.LoadPersonaReferences();

//read the persona references.
List<EvidenceReference> personaRefs = person.Person.Evidence;
```

<a name="read-discussion-references"/>

### Read Discussion References

```csharp
//the person on which to read the discussion references.
PersonState person = ...;

//load the discussion references for the person.
((FamilyTreePersonState)person).LoadDiscussionReferences();

//read the discussion references.
  List<DiscussionReference> discussionRefs = person.Person.FindExtensionsOfType<DiscussionReference>();
```

<a name="read-notes"/>

### Read Notes

```csharp
//the person on which to read the notes.
PersonState person = ...;

//load the notes for the person.
person.LoadNotes();

//read the discussion references.
List<Note> notes = person.Person.Notes;
```

<a name="read-relatives"/>

### Read Parents, Children, or Spouses

```csharp
//the person for which to read the parents, spouses, children
PersonState person = ...;

PersonChildrenState children = person.ReadChildren(); //read the children
PersonParentsState parents = person.ReadParents(); //read the parents
PersonSpousesState spouses = person.ReadSpouses(); //read the spouses
```

<a name="read-pedigree"/>

### Read Ancestry or Descendancy

```csharp
//the person for which to read the ancestry or descendancy
PersonState person = ...;

person.ReadAncestry(); //read the ancestry
person.ReadAncestry(QueryParameter.Generations(8)); //read 8 generations of the ancestry
person.ReadDescendancy(); //read the descendancy
person.ReadDescendancy(QueryParameter.Generations(3)); //read 3 generations of the descendancy
```

<a name="read-matches"/>

### Read Person Matches (i.e., Possible Duplicates)

```csharp
//the person for which to read the matches
PersonState person = ...;

PersonMatchResultsState matches = ((FamilyTreePersonState)person).ReadMatches();

//iterate through the matches.
List<Entry> entries = matches.Results.Entries;
```

<a name="declare-not-a-match"/>

### Declare Not a Match

```csharp
//the match results
PersonMatchResultsState matches = ...;

//iterate through the matches.
List<Entry> entries = matches.Results.Entries;

matches.AddNonMatch(entries.get[2], FamilySearchOptions.Reason("Because I said so."));
```

<a name="add-conclusion"/>

### Add a Name or Fact

```csharp
//the person to which to add the name, gender, or fact.
PersonState person = ...;

Name name = ...;
person.AddName(name.type(NameType.AlsoKnownAs), FamilySearchOptions.Reason("Because I said so.")); //add name
person.AddFact(new Fact(FactType.Death, "date", "place"), FamilySearchOptions.Reason("Because I said so.")); //add death fact
```

<a name="update-conclusion"/>

### Update a Name, Gender, or Fact

```csharp
//the person to which to update the name, gender, or fact.
PersonState person = ...;

Name name = person.Name;
name.NameForm.SetFullText("Joanna Smith");
person.UpdateName(name, FamilySearchOptions.Reason("Because I said so.")); //update name

Gender gender = person.Gender;
gender.SetKnownType(GenderType.Female);
person.UpdateGender(gender, FamilySearchOptions.Reason("Because I said so.")); //update gender

Fact death = person.Person.GetFirstFactOfType(FactType.Death);
death.SetDate(new DateInfo().SetOriginal("new date"));
person.UpdateFact(death, FamilySearchOptions.Reason("Because I said so."));
```

<a name="create-discussion"/>

### Create a Discussion

```csharp
FamilySearchFamilyTree ft = ...;

//add a discussion description
DiscussionState discussion = ft.AddDiscussion(new Discussion()
  //with a title.
  .SetTitle("What about this"),
  //with a change message
  FamilySearchOptions.Reason("Because I said so.")
);
```

<a name="create-discussion-reference"/>

### Attach a Discussion

```csharp
//the person that will be referencing the discussion.
PersonState person = ...;

DiscussionState discussion = ...;

((FamilyTreePersonState)person).AddDiscussionReference(discussion, FamilySearchOptions.Reason("Because I said so.");  //reference the discussion
```

<a name="attach-photo"/>

### Attach a Photo to a Person

```csharp
//the person to which the photo will be attached.
PersonState person = ...;
DataSource dataSource = ...; // Get a data source

//add an artifact
SourceDescriptionState artifact = person.AddArtifact(new SourceDescription()
  //with a title
  .SetTitle("Portrait of John Smith"),
  dataSource
);
```

<a name="read-fs-memories"/>

### Read FamilySearch Memories

```csharp
boolean useSandbox = true; //whether to use the sandbox reference.
String username = "...";
String password = "...";
String developerKey = "...";

//read the Family Tree
FamilySearchMemories fsMemories = new FamilySearchMemories(useSandbox)
  //and authenticate.
  .AuthenticateViaOAuth2Password(username, password, developerKey);
```

<a name="add-an-artifact"/>

### Upload Photo or Story or Document

```csharp
FamilySearchMemories fsMemories = ...;
DataSource digitalImage = ...;

//add an artifact
SourceDescriptionState artifact = fsMemories.AddArtifact(new SourceDescription()
  //with a title
  .SetTitle("Death Certificate for John Smith")
  //and a citation
  .SetCitation("Citation for the death certificate"), 
  digitalImage
);
```

<a name="create-memory-persona"/>

### Create a Memory Persona

```csharp
//the artifact from which a persona will be extracted.
SourceDescriptionState artifact = ...;

//add the persona
PersonState persona = artifact.AddPersona(new Person()
  //named John Smith
  .SetName("John Smith"));
```

<a name="create-persona-reference"/>

### Create a Persona Reference

```csharp
//the person that will be citing the record, source, or artifact.
PersonState person = ...;

//the persona that was extracted from a record or artifact.
PersonState persona = ...;

//add the persona reference.
person.AddPersonaReference(persona);
```

<a name="multi-person-photo-attach"/>

### Attach a Photo to Multiple Persons

```csharp
//the collection to which the artifact is to be added
CollectionState fsMemories = ...;

//the persons to which the photo will be attached.
PersonState person1 = ...;
PersonState person2 = ...;
PersonState person3 = ...;
DataSource digitalImage = ...;

//add an artifact
SourceDescriptionState artifact = fsMemories.AddArtifact(new SourceDescription()
  //with a title
  .SetTitle("Family of John Smith"),
  digitalImage
);

person1.AddMediaReference(artifact); //attach to person1
person2.AddMediaReference(artifact); //attach to person2
person3.AddMediaReference(artifact); //attach to person3
```
