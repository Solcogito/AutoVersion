# AutoVersion Integration Tests (TODO)

This directory is reserved for **end-to-end (E2E) integration tests** for the
AutoVersion CLI application.

Unlike unit tests—which validate individual components such as commands,
routing logic, or schema definitions—**integration tests simulate real usage**:

- Invoking the CLI exactly as a user would
- Running through `Program.Main(string[] args)`
- Tokenizing arguments via ArgParser
- Matching them against the schema
- Routing through CommandRouter
- Executing the corresponding command
- Writing output to stdout/stderr
- Producing the correct exit code

---

## Why integration tests?

Although unit tests provide strong coverage and reliability, integration tests
ensure that **all layers work correctly together**:
```
Program.cs
↓
ArgParser
↓
ArgSchema
↓
CommandRouter
↓
<Command>.Execute
↓
Console / Logger output
↓
Exit codes
```

## Integration tests help catch:

- Incorrect wiring between schema → router → commands  
- Parser regressions  
- Unexpected console output changes  
- Real-world error scenarios  

---

## Planned test cases

These tests will be implemented during a future development phase:

### 1. Bump command (patch/minor/major/prerelease)
- `autoversion bump patch`
- `autoversion bump prerelease -p alpha.1`

### 2. Current version
- `autoversion current`

### 3. Set version
- `autoversion set 1.2.3`
- `autoversion set 2.0.0 --dry-run`

### 4. Error scenarios
- unknown command: `autoversion potato`
- missing bump type: `autoversion bump`
- missing positional: `autoversion set`

### 5. Help tests
- no args: `autoversion`
- nested help requests: `autoversion bump --help`

---

## Temporary placeholder

Until implementation begins, this directory contains a placeholder test:

```csharp
[Fact(Skip = "Integration tests will be implemented later.")]
public void Placeholder() { }
```

This ensures:

- The folder is visible in the test suite

- The intent is documented

- No test failures occur

## When to implement these tests?
Integration tests should be added when:

- The CLI architecture is stable

- Logging API is finalized

- All major commands are in place

- You want automated regression guarantees

These tests are optional but highly valuable for long-term stability.

## Contributing
Please follow the same structure and test style conventions used in the unit
tests. Integration tests must:

- run deterministically

- not depend on environment state

- not modify real project files

- capture stdout/stderr safely

Mocking file paths or redirecting to a temporary test directory is recommended.
```
Solcogito S.E.N.C. — AutoVersion Lite
Integration Test Suite Placeholder
Version 0.8.0
```