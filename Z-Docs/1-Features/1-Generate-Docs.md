# Generate Docs

You can generate Typescript models from ArfBlocks code.

Command:
```sh
cd Arfware.ArfBlocks-cli
dotnet run generate-docs \
	--target-project ../Example/Backend/TodoApp.Application/TodoApp.Application.csproj \
	--output ../Example/Frontend/models/api/
```