FROM mhart/alpine-node:6.1.0

RUN mkdir /src
#install stuff we need
ADD . /src
#RUN rm -rf node_modules && npm cache clean

RUN cd /src && \
    npm install && \
    npm install -g mocha
  
RUN cd /src && \
    mocha  

#RUN cd /tests && \
#    grunt mochaTest


#Add files we need

#run tests
