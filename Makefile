.PHONY: test all documents
test:
	fd . -e cs Assets/Scripts/Pg \
		| xargs -I %% bash -c '[ "$$(head -1 %%)" = "#nullable enable" ] || echo "error: no nullable enable found in first line of [%%]"'
	fd '\b(Internal|Scene)\b' -t d Assets/Scripts/Pg \
		| xargs -I %% fd . -e cs %% \
		| grep -v -w -e Public -e Editor \
		| xargs perl -lne 'm{\b public \b}msx and print "error: public found [$$_] in $$ARGV"'
all: test documents
documents:
	fd . -e puml Documents | xargs plantuml
