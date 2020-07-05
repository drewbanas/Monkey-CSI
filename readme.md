Monkey CSI
---

A C\# port of the Monkey tree-walking interpreter from Thorsten Ball’s "*[Writing An Interpreter In Go](https://interpreterbook.com/)*".

Since Go and C\# have many differences, this port takes liberties on re-implementing some of the original Go code. Nonetheless, functions and variables remain where they are relative to each other, such that the Go and C\# code can still be compared side by side. Examples of changes include the following:

-   The interpreter can read script files. It was more convenient to write the tests in Monkey script files and look at the interpreter output, instead of embedding them as string literals in additional testing code. The extra code is in main, and recycles a lot from repl.

-   Successive if statements were used where Go used switch statements. Unlike C\#, Go’s switch allows expressions as cases.

-   Casting is used to access struct fields, whereas Go would just use them directly.

-   For Go functions that return tuples and have the second Boolean result as an  error indicator, null checking in C\# of the single result is used for error detection.

-   To make prefixing in the C\# code more consistent with the original Go implementation, some Go "import" statements do not have their corresponding C\# "using" statements. Since it was decided to use similarly named classes or structs in C\#, "using" statements can cause ambiguities with the namespace.

-   In place of what looks like function pointers in Go, C\# delegates were used for the Pratt parser. It turns out that instances of the assigned delegate lose scope of the fields in the class they belong to, if the fields are not static. Hence, fields and functions in the parser class are made static. This solved a bug wherein the tokens appear to be null within the parsing functions.

-   For the hash literals, instead of using a provided hashing function (there doesn’t seem to be one in Dot NET), the hash function from [clox](https://github.com/drewbanas/cloxcs) is recycled (Thanks Bob!). It’s just a few lines of code. Hence the additional file, “util.cs”.

-   “object” in lower case, is a keyword in C\#. It is capitalized to avoid clashing.

-   Go’s const block enumerations allows string members. This is approximated as a series of const strings (instead of going with the urge to use the usual more efficient integer based enums).

-   Instead of cramming function definitions within the arguments of Modify (the lost Chapter 5), the functions were separately defined first. A private field was used to pass the environment.

-   Since there’s no classes in Go, structs are also used except in cases where a link to the same type is needed (enclosing environment) or when assignment or comparison to null is needed.

-   Extra newlines were inserted in the initial REPL prompt (for personal reading preferences).

As of this writing, I still haven’t ever written a Go program. This supports the book’s claim that you don’t need to be a Go programmer to be able to follow through it.
