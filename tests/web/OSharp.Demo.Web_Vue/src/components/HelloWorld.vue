<template>
  <div class="hello">
    <h1>{{ msg }}</h1>
    <button @click="change">change</button>
    <function-component/>
  </div>
</template>

<script lang="ts">
import { HelloWorldService } from "./hello-world.service";
import { FunctionComponent } from "./function.component";

export default {
  name: "HelloWorld",
  data() {
    return {
      service: new HelloWorldService()
    };
  },
  methods: {
    change() {
      this.service.change();
    }
  },
  computed: {
    msg() {
      return this.service.msg;
    }
  },
  components: {
    FunctionComponent
  }
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
h1,
h2 {
  font-weight: normal;
}

ul {
  list-style-type: none;
  padding: 0;
}

li {
  display: inline-block;
  margin: 0 10px;
}

a {
  color: #42b983;
}
</style>
