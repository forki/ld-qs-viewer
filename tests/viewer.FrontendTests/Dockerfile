FROM mhart/alpine-node:6.1.0

#Add files we need
RUN mkdir /src
ADD . /src

#install stuff we need
RUN cd /src && \
    npm install && \
    npm install -g mocha
  
#run tests
RUN cd /src && \
    mocha  

