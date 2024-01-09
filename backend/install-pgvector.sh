#!/bin/bash

# Access the PostgreSQL container as root
docker exec -it -u root baza bash << 'EOF'
  # Inside the container, switch to the `postgres` user if necessary
  su postgres

  # Now access the PostgreSQL interactive shell with the default 'postgres' user
  psql -U postgres

  # Inside `psql`, you can run the SQL command to create the extension
  CREATE EXTENSION IF NOT EXISTS vector;
EOF
