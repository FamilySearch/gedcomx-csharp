# GEDCOM X Date
This is a spec-compliant implementation of [Gedcom X Dates](https://github.com/FamilySearch/gedcomx/blob/master/specifications/date-format-specification.md) with one exception: 5.3.2 - non-normalized durations.
It also includes some handy utility functions for manupulating ranges, durations, and recurring dates.

# NuGet Package Information


| Package ID | Link |
|------------|------|
| Gedcomx.Date | http://www.nuget.org/packages/Gedcomx.Date/ |

See [the section on using these libraries](../README.md#Use).

To determine the latest version visit the NuGet link listed above.

# Exceptions
Every error thrown is an instance of `GedcomxDateException`, which is a *runtime* exception.
Gedcomx Date was designed to give you and/or the end user as much information as possible.
For instance, when parsing fails, the message in the exception will tell you exactly what failed and where.

# Date Types
Each date extends the abstract class `GedcomxDate`.

````csharp
GedcomxDate date = GedcomxDateUtil.Parse("A+1900");

x = date.Type;
// returns GedcomxDateType.APPROXIMATE
// Very useful for case statements ;)

date.IsApproximate();
// returns true

date.ToFormalString();
// returns A+1900

````

**Simple**

The most basic of dates.
For example, `+1000`, `+2013-01-30`, `-0500-12-31T15:30:49`, and `+1987-03-25T01:00:00-06:00` are all examples of simple dates.

````csharp
GedcomxDateSimple simple = new GedcomxDateSimple("+1000");

x = simple.Year;
// returns 1000

x = simple.Month;
// returns null

simple.IsApproximate();
// returns false

simple.ToFormalString();
// returns +1000
````

**Approximate**

A Simple Date prepended with `A`.
For example, `A+1835` means approximately 1835.

````csharp
GedcomxDateSimple simple = new GedcomxDateSimple("A+1835");

x = simple.Year;
// returns 1835

x = simple.Month;
// returns null

simple.IsApproximate();
// returns true

simple.ToFormalString();
// returns +1835
````

**Range**

A Date range comprised of two simple dates or a simple date and a duration.
For example, `+1900/1910` means between 1900 to 1910.
Alternatively, `+1970-01-01T24:00:00/P3Y2D` means between midnight on January 1, 1970 and midnight on January 3, 1973.
A range may also be approximate (`A+2013-01/2014-03`), or only have one date (`/+1000` means up to the year 1000 and `+1970-01/` means starting January 1970).

````csharp
GedcomxDateRange range = new GedcomxDateRange("+1900/1910");

range.ToFormalString();
// returns +1900/1910

GedcomxDateSimple start = range.Start;
start.ToFormalDate();
// returns +1900

GedcomxDateSimple end = range.End;
end.ToFormalDate();
// returns +1910

GedcomxDateDuration duration = range.Duration;
duration.ToFormalDate();
// returns P10Y
````

**Recurring**

A Recurring date, specified by an optional number of times.
For example, `R3/+1900/P1Y` means this date starts in 1900, and recurs 2 times, ending in 1903.
`R/-500/P1Y5D`, `R500/+1000-01-01/+1001-03-15`, and `R/+1970-01-01T24:00:00/P3Y2D` are also recurring dates.

````csharp
GedcomxDateRecurring recurring = new GedcomxDateRecurring("R3/+1900/P1Y");

recurring.ToFormalString();
// returns R3/+1900/P1Y

GedcomxDateRange range = recurring.Range;
range.ToFormalString();
// returns +1900/P1Y

GedcomxDateSimple start = recurring.Start;
start.ToFormalDate();
// returns +1900

GedcomxDateSimple end = recurring.End;
end.ToFormalDate();
// returns +1903

GedcomxDateDuration duration = recurring.Duration;
duration.ToFormalDate();
// returns P1Y

GedcomxDateSimple nth = recurring.GetNth(15);
nth.ToFormalDate();
// returns +1915
````

**Duration**

A special sub-date of sorts, a duration represents the amount of time that has passed from a given starting date.
You can get the duration from a range or recurring

````csharp
GedcomxDateDuration duration = new GedcomxDateRecurring("P1Y35D");

duration.Years;
// returns 1

duration.Months;
// returns null

duration.Days;
// returns 35

duration.ToFormalString();
// returns P1Y35D
````

# Utility functions
All of these functions are static under the `GedcomxDateUtil` class

**parse(date)**
Parse a formal date string into the appropriate date type

````csharp
GedcomxDate date = GedcomxDateUtil.Parse("A+1900");
// date is an instance of GedcomxDateApproximate

GedcomxDate date = GedcomxDateUtil.Parse("R3/+1900/P1Y");
// date is an instance of GedcomxDateRecurring

GedcomxDate date = GedcomxDateUtil.Parse("Bogus");
// throws an instance of GedcomxDateException
````

**GetDuration(startDate, endDate)**
Get the duration between two simple dates. Throws an exception if start >= end.

````csharp
GedcomxDateDuration duration = GedcomxDateUtil.GetDuration(new GedcomxDateSimple("+1000"),new GedcomxDateSimple("+1100"));
// returns a duration of P100Y
````

**AddDuration(startDate, duration)**
Add a duration to a simple starting date and returns the resulting SimpleDate.

````csharp
GedcomxDateSimple date = GedcomxDateUtil.AddDuration(new GedcomxDateSimple("+1000"),new GedcomxDateDuration("P1Y3D"));
// returns a date of +1001-01-04
````


**MultiplyDuration(duration, multiplier)**
Multiply a duration by an integer value

````csharp
GedcomxDateduration duration = GedcomxDateUtil.MultiplyDuration(new GedcomxDateDuration("P1Y3D"),4);
// returns a date of P4Y12D
````

**DaysInMonth(month, year)**
Returns the number of days in the given month accounting for the year (leap year or not).

````csharp
int days;
days = DateTime.DaysInMonth(2003, 2);
// returns 28, non leap year
days = DateTime.DaysInMonth(2004, 2); 
// returns 29, leap year
days = DateTime.DaysInMonth(1900, 2);
// returns 28, non leap year
days = DateTime.DaysInMonth(2000, 2);
// returns 29, leap year
````