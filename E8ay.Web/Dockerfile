# base image
FROM node:8.10.0

WORKDIR /tmp
RUN npm install create-react-app@1.5.2 -g --silent

COPY ["package.json", "package-lock.json", "./"]
RUN npm install --silent

# set working directory
WORKDIR /app
RUN mv /tmp/node_modules ./ 


# start app
CMD ["npm", "start"]