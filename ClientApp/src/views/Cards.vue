<template>
  <v-container fluid>
    <div class="title_container">
      <v-row>
        <h2 style="margin-right: 10px">Active images</h2>
        <v-btn @click="addActiveImage()" style="width: 36px; min-width: 36px"
          ><v-icon>mdi-plus-circle-outline</v-icon></v-btn
        >
      </v-row>
    </div>
    <v-row>
      <v-card
        v-for="n in nbActiveImages"
        v-bind:key="n"
        style="margin: 5px"
        ><div class="frame">
          <v-img
            class="card_image"
            v-bind:class="{ active: isActiveIndex(n) }"
            v-bind:src="activeImages[n - 1]"
            v-on:click="setIndex(n)"
          ></v-img>
        </div>
        <v-card-text
          >[{{ n }}] :
          {{
            activeImages[n - 1] === "empty.png"
              ? "&lt;empty&gt;"
              : getImageName(activeImages[n - 1])
          }}</v-card-text
        >
        <v-card-actions>
          <v-btn style="width: 100%" @click="getLiveUrl(n)">
            <v-icon>mdi-content-copy</v-icon
            ><span>&nbsp;&nbsp;Copy live URL</span>
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-row>
    <div class="title_container" style="margin-top: 24px">
      <v-row>
        <h2 style="margin: 1px 10px 1px 0px">Set active image</h2>
        <v-btn
          v-if="showLocalFolder"
          @click="openImageFolder()"
          style="width: 36px; min-width: 36px"
          ><v-icon>mdi-folder-open</v-icon></v-btn
        >
        <v-btn
          @click="fetchImages()"
          style="width: 36px; min-width: 36px"
          ><v-icon>mdi-refresh</v-icon></v-btn
        >
      </v-row>

      <v-row>
        <v-col cols="12" md="4">
          <v-row>
            <v-text-field v-model="activeImageUrl" label="Image from URL :">
              <template v-slot:append>
                <v-btn
                  @click="setActiveImage(activeImageUrl)"
                  style="width: 36px; min-width: 36px"
                  ><v-icon>mdi-link-variant-plus</v-icon></v-btn
                >
                <v-btn
                  @click="downloadImage(activeImageUrl)"
                  style="width: 36px; min-width: 36px"
                  ><v-icon>mdi-download</v-icon></v-btn
                >
              </template></v-text-field
            >
          </v-row>
        </v-col>
      </v-row>
    </div>

    <v-row>
      <v-tabs background-color="blue" dark v-model="tab">
        <v-tab v-for="imageSet in imageSets" v-bind:key="imageSet.name">
          {{ imageSet.name }}
        </v-tab>
      </v-tabs>
      <v-tabs-items v-model="tab">
        <v-tab-item
          v-for="imageSet in imageSets"
          v-bind:key="imageSet.name"
          class="tab_content"
        >
          <v-row>
            <v-col v-for="image in imageSet.images" v-bind:key="image">
              <img
                v-bind:src="image"
                v-bind:class="{
                  active: isActiveImage(image),
                }"
                v-on:click="setActiveImage(image)"
                style="max-width: 300px; border-radius: 5px"
              />
              <p>{{ getImageName(image) }}</p>
            </v-col>
          </v-row>
        </v-tab-item>
      </v-tabs-items>
    </v-row>
  </v-container>
</template>

<script lang="ts">
// an example of a Vue Typescript component using Vue.extend
import Vue from "vue";
import axios from "axios";

interface image {
  filename: string;
  index: number;
}

export default Vue.extend({
  data() {
    return {
      tab: 0 as number,
      loading: true,
      showError: false,
      errorMessage: "",
      imageSets: [],
      activeImages: ["/empty.png"],
      index: 1 as number,
      showLocalFolder: false,
      activeImageUrl: "",
    };
  },
  computed:
  {
    nbActiveImages() : number {
      return this.activeImages.length;
    }
  },
  methods: {
    async fetchImages() {
      try {
        const response = await axios.get("api/Cards");
        this.imageSets = response.data;
      } catch (e) {
        this.showError = true;
        this.errorMessage = `Error while loading images: ${e.message}.`;
      }
      this.loading = false;
    },
    async fetchActiveImages() {
      try {
        const response = await axios.get<image[]>("api/cards/active");
        let activeImagesArray = ["/empty.png"];
        response.data.forEach((element) => {
          activeImagesArray[element.index - 1] = element.filename;
        });
        for (let i = 0; i < activeImagesArray.length; i++) {
          if (activeImagesArray[i] === undefined) {
            activeImagesArray[i] = "/empty.png";
          }
        }
        this.activeImages = activeImagesArray;
      } catch (e) {
        this.showError = true;
        this.errorMessage = `Error while loading images: ${e.message}.`;
      }
      this.loading = false;
    },
    async setActiveImage(image: string) {
      try {
        const response = await axios.post("api/Cards", {
          filename: image,
          index: this.index,
        });

        this.activeImages.splice(this.index - 1, 1, image);
      } catch (e) {
        this.showError = true;
        this.errorMessage = `Error while setting active image ${e.message}.`;
      }
    },
    async downloadImage(image: string) {
      try {
        const response = await axios.post("api/Cards/download", {
          filename: image,
          index: this.index,
        });
        this.fetchImages();
        this.tab = 1;
      } catch (e) {
        this.showError = true;
        this.errorMessage = `Error while setting active image ${e.message}.`;
      }
    },
    isActiveImage(image: string) {
      return this.activeImages[this.index - 1] === image ? true : false;
    },
    isActiveIndex(n: number) {
      return this.index === n ? true : false;
    },
    setIndex(n: number) {
      this.index = n;
    },
    addActiveImage() {
      this.activeImages.push("empty.png");
    },
    getImageName(image: string) {
      const pattern = new RegExp(/^.*\/(.*)\..*$/);
      const match = pattern.exec(image);
      return match === null ? "" : match[1];
    },
    getLiveUrl(n: number) {
      let port: string = "";
      if (location.port != "80" && location.port != "443") {
        port = `:${location.port}`;
      }
      navigator.clipboard.writeText(
        `${window.location.hostname}${port}/live/image/${n}`
      );
    },
    openImageFolder() {
      try {
        axios.post("api/Cards/openImageFolder");
      } catch (e) {
        this.showError = true;
        this.errorMessage = `Error while opening local image folder ${e.message}.`;
      }
    },
    isLocal() {
      console.log(location.host);
      if (location.host.includes("localhost")) {
        this.showLocalFolder = true;
      }
    },
  },
  async created() {
    this.isLocal();
    await this.fetchImages();
    await this.fetchActiveImages();
  },
});
</script>
<style scoped>
.active {
  border-color: rgba(82, 168, 236, 0.8);
  box-shadow: 0px 0px 8px 8px rgba(82, 168, 236, 0.8);
  outline: 0 none;
}
.tab_content {
  margin: 15px;
}
.frame {
  height: 100px;
  width: 200px;
  margin: 10px;
  position: relative;
}
.title_container {
  padding: 12px;
  margin: 12px;
}
.card_image {
  max-height: 100%;
  max-width: 100%;
  width: auto;
  height: auto;
  position: absolute;
  top: 0;
  bottom: 0;
  left: 0;
  right: 0;
  margin: auto;
  border-radius: 5px;
}
</style>
