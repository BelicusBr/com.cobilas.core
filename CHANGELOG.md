# Changelog
## [3.0.0] - 29/08/2023
### Removed
- Os metódos obsoletos da classe `ALFBTWriter`
```c#
public abstract void WriterMarkingFlag(string, string);
public abstract void WriterTextFlag(string, string);
```
foram removidos.
- Os metódos obsoletos da classe `ALFBTRead`
```c#
public abstract HeaderFlag GetHeaderFlag();
public abstract MarkingFlag GetMarkingFlag(string);
public abstract TextFlag GetTextFlag(string);
public abstract MarkingFlag[] GetAllMarkingFlags();
public abstract TextFlag[] GetAllTextFlags();
public abstract bool TextFlagExists(string);
public abstract bool MarkingFlagExists(string);
public abstract bool HeaderFlagExists(string);
```
foram removidos.
### Changed
- O metódo `bool ALFBTRead.FlagExists(string, AlfbtFlags);` deixou de ser obsoleto.
### Added
- O metódo `IItemReadOnly ALFBTMemoryStreamRead.GetFlag(string, AlfbtFlags);` foi adicionado.
## [2.0.0-ch1] - 28/08/2023
### Changed
- O autor do pacote foi alterado de `Cobilas CTB` para `BélicusBr`.
## [1.0.3] - 30/01/2023
### Added
- Formato '1.5' ALFBT
- A estrutura de leitura 'ALF' foi adicionada.
- Um nova estrutura de bancode tradução foi adicionada.
### Changed
- As estruturas de leitura 'ALFBT' agora são abstratas.
- Ouve uma pequena alteração na forma de auto detecção da versão do formato alfbt.
- Agora o construtor `LanguageInfo.LanguageInfo(string, string)` é publico.
### Fixed
- Na `string:LanguageCollection.GetLanguageText(string)` o texto resultante agora e obtido do campo `language` em vez de `comunaManifest`.
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
