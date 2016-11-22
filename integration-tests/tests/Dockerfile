FROM nice/node-phantomjs

# Install app dependencies
ADD package.json /tmp/package.json
RUN cd /tmp && npm install
RUN mkdir -p /app && cp -a /tmp/node_modules /app/

WORKDIR /app
COPY features/ /app/features

ADD wait-for-it.sh /app

# run the tests
CMD ["./node_modules/.bin/cucumber.js"]
