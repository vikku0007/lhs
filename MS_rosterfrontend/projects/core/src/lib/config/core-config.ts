import { InjectionToken } from '@angular/core';

export const CORE_CONFIG = new InjectionToken<CoreConfig>('core.config');
export interface CoreConfig {
    baseUrl?: string;
}
