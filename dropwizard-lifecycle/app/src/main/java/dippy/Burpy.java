package dippy;

import io.dropwizard.lifecycle.Managed;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

public class Burpy implements Managed {
    private static final Logger log = LoggerFactory.getLogger(Burpy.class);

    String label;

    public Burpy(String label) {
        this.label = label;
    }

    @Override
    public void start() throws Exception {
        log.info("^^^ START {}", label);
    }

    @Override
    public void stop() throws Exception {
        log.info("^^^ STOP {}", label);
    }
}
