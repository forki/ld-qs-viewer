FROM mhart/alpine-node:6.1.0

#install stuff we need
ADD package.json package.json
ADD Gruntfile.js Gruntfile.js
ADD tests/ tests/

RUN npm install && \
    npm install -g grunt-cli

RUN cd /tests && \
    grunt mochaTest


#Add files we need

#run tests
