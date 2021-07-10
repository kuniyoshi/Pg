test:
	fd . -e cs Assets/Scripts/Pg \
		| xargs -I %% bash -c '[ "$$(head -1 %%)" = "#nullable enable" ] || echo "error: no nullable enable found in first line of [%%]"'
	fd Internal -t d Assets/Scripts/Pg \
		| xargs -I %% fd . -e cs %% \
		| xargs perl -lne 'm{\b public \b}msx and print "error: public found [$$_] in $$ARGV"'
