package dippy;

import io.dropwizard.ConfiguredBundle;
import io.dropwizard.setup.Bootstrap;
import io.dropwizard.setup.Environment;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

public class SampleBundle implements ConfiguredBundle<DippyConfiguration> {
    private static final Logger log = LoggerFactory.getLogger(SampleBundle.class);

    @Override
    public void initialize(Bootstrap<?> bootstrap) {
        log.info("**  BUNDLE.INIT");
        System.out.println("**  BUNDLE.INIT START");


        System.out.println("** /BUNDLE.INIT STOP");
        log.info("** /BUNDLE.INIT");
    }

    @Override
    public void run(DippyConfiguration configuration, Environment environment) throws Exception {
        log.info("**  BUNDLE.RUN");
        log.info("** /BUNDLE.RUN ");
    }


}
