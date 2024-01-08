#!/bin/bash

# Access the container as root
winpty docker exec -it -u root baza bash -c '
  # Update package lists
  apt-get update

  # Install the necessary build dependencies
  apt-get install -y git build-essential postgresql-server-dev-16 postgresql-16

  # Clone the pgvector repository (choose the appropriate branch/tag if necessary)
  git clone https://github.com/ankane/pgvector.git

  # Enter the source directory and compile pgvector
  cd pgvector && make && make install

  # Exit the source directory
  cd ..

  # Clean up if desired
  rm -rf pgvector

  # Inside the container, switch to the `postgres` user if necessary
  su postgres

  # Now access the PostgreSQL interactive shell with the default 'postgres' user
  psql -U postgres <<'PSQL_EOF'
    # Inside `psql`, you can run the SQL command to create the extension
    CREATE EXTENSION IF NOT EXISTS vector;
PSQL_EOF
'
