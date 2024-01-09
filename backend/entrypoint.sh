#!/bin/bash

./install-pgvector.sh

dotnet ef database update

exec dotnet backend.dll
