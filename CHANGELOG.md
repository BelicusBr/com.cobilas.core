# Changelog
## [1.0.3] - 30/01/2023
### Added
- Formato '1.5' ALFBT
- A estrutura de leitura 'ALF' foi adicionada.
- Um nova estrutura de bancode tradução foi adicionada.
### Changed
- As estruturas de leitura 'ALFBT' agora são abstratas.
### Deprecated
- Formato '1.0' ALFBT.
- A estrutura de bancode tradução `TranslationManagement` foi substituida por `LanguageManager`.
#### deprecated functions
- `ALFBTWriter.WriterTextFlag(string, string);`
- `ALFBTWriter.WriterMarkingFlag(string, string);`</br>
Use a função `ALFBTWriter.WriteElement(string, string)` para escrever uma nova bamdeira.

## [1.0.1] - 15/01/2023
### Add
- `class PrintOut`
- `class TypeUtilitarian`
- `interface IReadOnlyArray`
- `interface IReadOnlyArray<T>`

Classe de leitura de arquivos com formato `.alf` adicionadado.
- `namespace Cobilas.IO.Alf`

### Obsolete
- `class CobilasConsole`
- `class CobilasTypeUtility`
### Change
- `class ALFBTWrite` > `class ALFBTWriter`

## [1.0.0] - 11/11/2022
### Repositorio com.cobilas.core iniciado
- Lançado para o GitHub
