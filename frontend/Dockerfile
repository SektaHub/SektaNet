FROM node:18-alpine

# Set the working directory in the container
WORKDIR /app

# Copy package.json and package-lock.json (or npm-shrinkwrap.json, yarn.lock)
COPY package*.json ./

# Install dependencies
RUN npm install

# Copy the rest of the application files to the container
COPY . .

# Build the TypeScript code
RUN npm run build

# Install serve globally for production deployment
RUN npm install -g serve

# Expose the port on which your application will run
EXPOSE 3000

# Set the NODE_ENV to production
ENV NODE_ENV=production

# Command to run the application using serve
CMD ["serve", "-s", "dist", "-l", "3000"]