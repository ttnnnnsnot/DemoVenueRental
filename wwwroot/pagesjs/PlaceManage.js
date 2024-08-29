import { setupLayout, layoutOption } from '../pagesjs/Layout.js';

import indexBannerOption from '../components/IndexBanner.js';
import {
    indexSelectTypeOption,
    indexSelectMore,
    search as searchFunction
} from '../components/IndexSelectType.js';

const appOption = {
    components: {
        // Layout.js
        ...layoutOption.components,
        // Index.js
        'index-select-type': indexSelectTypeOption,
        'index-banner': indexBannerOption,
    },
    setup() {
        // Layout.js
        const { registerComponent, showRegisterModal,
            loginComponent, showLoginModal,
            Logouted,
            LoggedIn,
            LayoutonBeforeMount,
            LayoutonMounted,
            headerCurrentState } = setupLayout();

        onBeforeMount(async () => {
            await LayoutonBeforeMount();
        });

        onMounted(async () => {
            await LayoutonMounted();
            headerCurrentState.value = 2;
        });

        // PlaceManage.js
        const LinkEditUrl = async (id) => {
            if (id === 0) {
                location.href ='/home/PlaceEdit';
            }
        }

        return {
            // Layout.js
            showRegisterModal, registerComponent,
            loginComponent, showLoginModal,
            LoggedIn,
            Logouted,

            // PlaceManage.js
            LinkEditUrl
        }
    }
};

const app = createApp(appOption);

app.mount("#app");