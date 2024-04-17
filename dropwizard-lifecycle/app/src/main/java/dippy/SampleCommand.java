package dippy;

import io.dropwizard.cli.EnvironmentCommand;
import io.dropwizard.setup.Environment;
import net.sourceforge.argparse4j.inf.Namespace;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.util.ServiceLoader;

public class SampleCommand extends EnvironmentCommand<DippyConfiguration> {
    private static final Logger log = LoggerFactory.getLogger(SampleCommand.class);

    public SampleCommand(App app) {
        super(app, "lifer","lifecycle test");
    }

    @Override
    protected void run(Environment environment, Namespace namespace, DippyConfiguration configuration) throws Exception {
        log.info("** CMD.RUN");
        System.out.println(" CMD.RUN");

        System.out.println("/CMD.RUN");
        log.info("** /CMD.RUN");
    }
}
