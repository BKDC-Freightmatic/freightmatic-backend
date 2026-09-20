# Use official Node.js 20 runtime as parent image
FROM node:20

# Set working directory
WORKDIR /usr/src/app

# Copy package management files
COPY package.json yarn.lock* ./

# Install dependencies
RUN yarn install || npm install

# Copy application source code
COPY . .

# Expose app port
EXPOSE 4000

# Command to run the NestJS application
CMD ["npm", "run", "start:dev"]
