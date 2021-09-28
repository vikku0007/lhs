import { CoreConfig } from 'projects/core/src/lib/config/core-config'
import { environment } from 'src/environments/environment'

export const CoreConfigFactory = () => {
    return { baseUrl: environment.baseUrl } as CoreConfig;
}
