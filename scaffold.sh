#!/bin/bash
~/.dotnet/tools/dotnet-ef dbcontext scaffold Name=ConnectionStrings:Default Microsoft.EntityFrameworkCore.SqlServer -o Models --force --project Data -s WebApi
