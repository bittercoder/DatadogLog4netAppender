Log4net appender for datadoghq
------------------------------

Datadoghq (http://www.datadoghq.com/) provides a mechanism for posting events to their server via an API.  This log4net appender takes advantage of that so you can push subets of errors/notifications to your datadoghq event stream.

This log4net appender uses the message & logger name to calculate an aggregate key (made up of a SHA256 hash of the message+logger name, prefixed with a portion of the message).  

The aggregate key goes through a guid and integer remapper, to ensure we group messages together which are being reported for different entities, but which are the same "class" of error.

This library is licensed under Apache 2

http://www.apache.org/licenses/LICENSE-2.0.html