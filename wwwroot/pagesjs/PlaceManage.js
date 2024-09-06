import { setupLayout, layoutOption } from '../pagesjs/Layout.js';

const appOption = {
    components: {
        // Layout.js
        ...layoutOption.components,
    },
    setup() {
        // Layout.js
        const {
            registerComponent,
            loginComponent,
            isLoggedIn,
            layoutLogouted,
            layoutLoggedIn,
            layoutOnBeforeMount,
            layoutOnMounted,
            headerCurrentState,
            checkPathName } = setupLayout();

        onBeforeMount(async () => {
            await layoutOnBeforeMount();
        });

        onMounted(async () => {
            await layoutOnMounted();
            // PlaceManage.js
            changeHeaderState();
        });

        const performLoggedIn = async () => {
            await layoutLoggedIn();

            // PlaceManage.js
            changeHeaderState();
        }

        const performLogouted = async () => {
            await layoutLogouted();

            // PlaceManage.js
            changeHeaderState();
        }

        // PlaceManage.js
        const changeHeaderState = () => {
            headerCurrentState.value = isLoggedIn.value ? 2 : 1;
        }

        const linkEditUrl = async (id) => {
            if (id == 0) {
                await API.DELETE('Place/PlaceId');
                checkPathName("/home/placeedit");
            }
        }

        // Layout.js
        provide('loggedIn', performLoggedIn);
        provide('logouted', performLogouted);

        return {
            // Layout.js
            registerComponent,
            loginComponent,

            // PlaceManage.js
            linkEditUrl
        }
    }
};

const app = createApp(appOption);

app.mount("#app");