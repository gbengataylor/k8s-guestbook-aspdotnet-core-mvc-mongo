FROM debian:9
RUN  apt-get update && apt-get -qq -y install libunwind8 libicu57 libssl1.0 liblttng-ust0 libcurl3 libuuid1 libkrb5-3 zlib1g
COPY * /exe/
CMD /exe/GuestbookWebApi 
