#!/bin/bash

set -e
set -u

####
# Notes:
#   superuser and createdb access faciliate unit testing more than anything

psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" --dbname "$POSTGRES_DB" <<-SQL
    CREATE USER testing WITH PASSWORD 'testing' superuser createdb;
    CREATE DATABASE testing;
    GRANT ALL PRIVILEGES ON DATABASE testing TO testing;
SQL