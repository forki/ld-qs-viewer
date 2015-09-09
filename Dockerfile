FROM nice/ld-docker-build 

MAINTAINER James Kirk <james.kirk84@gmail.com>

COPY . /etc/ld-viewer/

CMD /etc/ld-viewer/build.sh
